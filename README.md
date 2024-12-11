# Vocabkitchen

https://vocabkitchen.com/

## Domain Overview

This is a project that came out of my work as a language teacher. It allows an ESL student or teacher to find the CEFR vocabulary level of a piece of text. The application allows teachers to take a text, adjust it to a specific vocabulary level, and then collect data on unknown words in the text while automatically creating targeted learning activities for each student.

As a SAAS product, it never gained traction as it was challenging to identify a customer/purchasing decision maker in an educational system for a small, specialized platform like this. The vocabulary profiler as a free service has been successful (in the sense that people miss it when it goes down) and has had, for several years, a global user base of about 5000 unique yearly visitors. Currently I maintain the free version of the site with hosting paid by an educational institution in the UAE.

## Technical Overview

I've made this repository public to showcase the programming work I did on the biggest version of the application. Overall, I rewrote vocabkitchen.com four times from 2012 to 2024: from .NET Webforms to .NET MVC/AngularJS to .NET Core 3.1/Angular 9 with a final downsizing rewrite in .NET 8 and React 18. Unfortunately, most of what I describe below was removed from the site in the last rewrite so a demo isn't publicly available.

Here is a tour of some of the highlights of how this application was built at its peak complexity, which is captured in this repo.

**Clean Architecture**

The monolithic .NET application contains four projects:

`VkCore`: contains domain interfaces and classes. This project is "clean" in that it has very minimal depedencies on external packages and is a pure object-oriented representation of the business domain. I did make the exception that the EF `DbContext` is injected into methods that need to do data manipulation.

`VkInfrastruction`: handles I/O behavior like reading from the file system, sending email, third party API access, and database access

`VkWeb`: a gateway to the infrastructure and domain logic via an API. It also bundles and serves the Angular client application.

`VkWeb/ClientApp`: an Angular 9 application that provides a GUI over the VkWeb API

**Domain Driven Design**

The application uses domain driven design to model the behavior of teaching new vocabulary words from a text to a group of students. At the heart of this domain there is a reading (a text), and words or phrases in the reading which students can understand (or not), and word definitions which need to help students gain understanding of the word's meaning, pronunciation and spelling.

So the domain requirements are:

1. adding and editing a text
1. identifying what words students don't know
1. adding definitions to the words and capturing example sentences from the text
1. providing repeated practice of the words

The heart of this design is the `Reading` class in `VkCore/Models/ReadingModel/Reading.cs`. This was loosely inspired by some homework I did on how Google Docs is built. A `Reading` consists of a collection of `ContentItems` (in hindsight, that could've had a better name). A `Reading` is text broken up into a collection of strings (`ContentItems`) each with a index that, when sorted in order, allows the `Reading` to output the complete text body from its parts.

For example, here is a text:

Text: "He is sad."
Index: 0123456789

To model this as a `Reading` you could have a single content area starting at index 0 with the `value` of "He is sad." Then, to define "sad", you would need to split the text into 3 `ContentItems`. The first would start at index 0 and contain "He is ", the second has index 6 and contains "sad", and the last has index 9 and contains ".". Once we've split out the `ContentItems`, we're then able to attach definitions to them.

As a user edits the text in a text area and adds definitions, the `ContentItems` need to shift their indexes, or split and collapse `ContentItems`. The methods on `Reading` handle this behavior, and in the spirit of tests as documentation, you can see more about how this works in `VkCore.Test/Models/ReadingShould.cs`

I should note that the client-side implementation of this was a custom-build text editor in Angular (`VkWeb/ClientApp/src/app/org-dashboard/reading-edit/reading-edit.component.ts`) where a `ContentItem` roughly maps to an `Edit` (`VkWeb/ClientApp/src/app/org-dashboard/models/edit.ts`). I won't go into detail here, but that Angular component also handles undo/redo and captures the index of all edits made by the cursor/keyboard so they can be captured appropriately by the backend model and is a good example of using RxJS and event listeners to capture keyboard and mouse input.

For now, that is a solid example of how a domain problem (editing a text and adding word defintions) was modeled using C# classes. I'll walk through the run-time behavior of this code in the next section.

**The Mediator Pattern**

The application uses the [mediator pattern](https://github.com/jbogard/MediatR/wiki) to connect API requests to the domain logic and infrastructure defined in the project. I found this pattern to be slightly more _functional_ than the sometimes bloated service classes you find in n-tier .NET applications, for example, you could imagine a `ReadingService` class, but with our domain models, I think that would muddy the waters too much. It also reduces the temptation to write [god objects](https://en.wikipedia.org/wiki/God_object) in a such a service class, and forces you to think in terms of single-responsibility. One trade of is that tracing behavior orchestration can be more complicated.

An interesting example is adding a definition to a text and tracking student knowledge of that definition. The requirements were:

1. given word polysemy, the definition needed to be connected a specific word definition at a specific index range in a text
1. we should capture the sentence that contained the word or phrase from the text so we can use it in learning exercises
1. in adition to generating to capturing the definition, we need to generate an audio file of the pronunciation if we don't already have one.

Given the domain models (`Reading` and `ContentItem`) from the above section, this is how adding a definition would flow through the application.

1. The user chooses some definition values in `VkWeb/ClientApp/src/app/org-dashboard/definition-modal/definition-modal.component.ts`. These values are posted back to the API via the Angular `ReadingService`
1. The values from the client come into the API (`ReadingController.cs`) as an `AddDefinitionRequest`. The mediator pattern leads to very thin controller actions, and in the case all we do here is grab the user id from the request context and pass the data off to `MediatR`.
1. `MediatR` matches our request to the `AddDefinitionRequestHandler`. In this application I use service classes to perform specific tasks, for example, pulling a complete sentence from a `Reading` at a specific index (`VkCore/Services/ExampleSentenceService.cs`) The request handlers here do several jobs: 1) do data validation with [guard clauses](<https://en.wikipedia.org/wiki/Guard_(computer_science)>) as needed, 2) pull back any data needed to hydrate the domain model. In this case we use the [builder pattern](https://en.wikipedia.org/wiki/Builder_pattern) - `DefinitionBuilder` - to construct our definition object which we finally pass off to `Reading.InsertDefinition` on our domain model to persist the new definition.
1. An interesting side effect to note here, part of our domain model is a `Word`, which is the long running, dictionary entry equivalent of a `ContentItem`. So when we add a definition to a content item, for example with the value "sad", we also create a "sad" `Word` entry if needed: this is the part of the domain that students study over time. In this case, the `AddDefinitionRequestHandler` conditionally calls the `WordEntry` domain model, which in its constructor fires off a `WordAddedEvent`
1. A nice feature of the event pattern in mediator is the ability to write multiple single-responsibility handlers to respond to the event, reducing the need to write orchestration code. In this case, the `WordAddedAudioCreationHandler` event fires, sending off a `CreateWordAudioRequest`, which fires the `CreateWordAudioRequestHandler` that calls an AWS lambda to asychronously calls a text to speech API and dumps an audio file in an S3 bucket which the client application can use later to play word audio.
