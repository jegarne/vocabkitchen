import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'issue-log',
  templateUrl: './issue-log.component.html',
  styleUrls: ['./issue-log.component.css']
})
export class IssueLogComponent implements OnInit {

  issues = [];

  constructor() { }

  ngOnInit() {
    this.issues.push({
      date: 'April 25, 2020', title: 'Improved Definition Removal',
      description: `Definitions now show how many times they are used throughout the site.  Definitions that are
                    used on only one word are now completely deleted when you remove them.`
    });
    this.issues.push({
      date: 'April 25, 2020', title: 'Fixed Password Reset From',
      description: `Fixed error that prevented users from resetting password.`
    });
    this.issues.push({
      date: 'April 13, 2020', title: 'Bug Fixes and Improvements',
      description: `Prevent saving empty word definitions and bug fix that prevented texts with word definitions from
                    being edited.`
    });
    this.issues.push({
      date: 'March 6, 2020', title: 'Playing sound in Safari browsers',
      description: `Sound not playing on Apple/iOS devices is a known issue.  I'm working on a rewrite of how sound is
                    handled in the app and it should be available in a few days.  It should also improve responsiveness of
                    audio for users outside of North America.`
    });
    this.issues.push({
      date: 'March 6, 2020', title: 'Add the New Academic Word List profiler',
      description: `This was by request.  I'm not very familiar with this list and have left up
                    the original AWL list so both are still available.`
    });
    this.issues.push({
      date: 'March 2, 2020', title: 'Bug Fixes and Improvements',
      description: `Added information to the UI to help with navigation.
                    Fixed word progress data.`
    });
    this.issues.push({
      date: 'March 1, 2020', title: 'Add Table Result to CEFR Word Doc Download',
      description: `The CEFR Word Doc download now contains the results in table format.`
    });
    this.issues.push({
      date: 'February 26, 2020', title: 'Download Profiler Results to Word Doc',
      description: `You can download CEFR results as a Word doc.  This has currently only be tested in
                    Chrome.  If you have issues in other broswers or devices please send feedback.`
    });
    this.issues.push({
      date: 'February 22, 2020', title: 'Bounced Email Cleanup',
      description: `Student or teacher invitation emails that can't be sent are marked as bad in the "Users" tab.
                    Users that sign up with bad emails will be deleted.  As part of this clean up,
                    any users that have not confirmed their emails were deleted.  Please sign up
                    again with a good email.`
    });
    this.issues.push({
      date: 'February 22, 2020', title: 'Admins Get Added as Students',
      description: `When new admin users create an organization, they are automatically added as students.`
    });
    this.issues.push({
      date: 'February 20, 2020', title: 'Default Tags',
      description: `A tag can be set as default.  All new readings and users are automatically set with the default tag.
                    Also, a default tag is created and added when a new reading or user is added and no tags exist.`
    });
    this.issues.push({
      date: 'February 19, 2020', title: 'Student Limit Increase Requests',
      description: `Allow users that want to trial the application with more than 60 students to request an increase.`
    });
    this.issues.push({
      date: 'February 18, 2020', title: 'Select All Students',
      description: `Added a checkbox that allows you to select all students and add them to a tag.`
    });
    this.issues.push({
      date: 'February 17, 2020', title: 'Error Saving Readings',
      description: `Fixed a bug that was not allowing readings to be saved.`
    });
    this.issues.push({
      date: 'February 14, 2020', title: 'Bug Fixes',
      description: `Fixed sorting on student and word tables.`
    });
    this.issues.push({
      date: 'February 14, 2020', title: 'Word Attempt Summary',
      description: `You can now view a summary of all words you've defined accross your readings and how many student attempts and failures have
                    taken place.  This should be useful for guiding what words to review.`
    });
    this.issues.push({
      date: 'February 14, 2020', title: 'Student Invite Process',
      description: `When students or teachers are invited by an admin, they will no longer receive a second email requesting them to confirm their email.
                    They should be able to set up their new account and log in immediately.  The initial Admin user will still need
                    to confirm his or her email.`
    });
    this.issues.push({
      date: 'February 14, 2020', title: 'CEFR Results Enhancements',
      description: `Updated level colors for improved contrast.  You can now select levels individually by clicking on the level in the key (they're buttons).`
    });
    this.issues.push({
      date: 'February 11, 2020', title: 'Add New Tags from Readings',
      description: `When adding new tags for a reading, create the tag you typed if it doesn't already exist.`
    });
    this.issues.push({
      date: 'February 11, 2020', title: 'Password Confirmation Updates',
      description: `Require invitees to confirm their password when creating accounts.  Require users to confirm their
                    passwords when changing their password inside the app.`
    });
    this.issues.push({
      date: 'February 10, 2020', title: 'New Email Provider',
      description: `The vocabkitchen.com domain was set up and verified to send emails.  I would really appreciate feedback
                    if you still receive our emails in your spam folder.  All emails from this site will be sent from
                    contact@vocabkitchen.com.  You can also email me directly at that address.`
    });
    this.issues.push({
      date: 'February 3, 2020', title: 'Profiler Enhancements',
      description: `Reorganized the home page and navigation so it's clear how to get back to the profiler when signed in,
                    updated profiler UI to improve readability, improved profiler punctuation handling.`
    });
    this.issues.push({
      date: 'January 26, 2020', title: 'Bug Fixes and Enhancements',
      description: `Addressed issue: error messages were not displaying correctly, added password confirmation field,
            made it possible to request another email confirmation message.`
    });
    this.issues.push({
      date: 'January 21, 2020', title: 'New Version Launched',
      description: `Share your leveled reading texts with students, add definitions for words
      in context (both manually and pulled from dictionaries), automatically generate vocabulary learning exercises,
      and track student progress.`
    });
  }

}
