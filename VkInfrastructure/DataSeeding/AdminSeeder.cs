using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using VkCore.Builders;
using VkCore.Constants;
using VkCore.Interfaces;
using VkCore.Models;
using VkCore.Models.Organization;
using VkCore.Models.ReadingModel;
using VkCore.Models.Word;
using VkCore.Requests.User;
using VkInfrastructure.Data;

namespace VkInfrastructure.DataSeeding
{
    public interface IAdminSeeder
    {
        Task Seed();
    }

    public class AdminSeeder : IAdminSeeder
    {
        private readonly UserManager<VkUser> _userManager;
        private readonly VkDbContext _context;
        private readonly IVkConfig _config;
        private readonly ITagService _tagService;

        public AdminSeeder(
            UserManager<VkUser> userManager,
            VkDbContext context,
            IVkConfig config,
            ITagService tagService
        )
        {
            _userManager = userManager;
            _context = context;
            _config = config;
            _tagService = tagService;

        }

        public async Task Seed()
        {

            var existing = await _userManager.FindByEmailAsync("admin@email.com");
            if (existing != null) return;

            var adminUserRequest = new CreateUserRequest()
            {
                FirstName = "John",
                LastName = "McAdmin",
                Email = "admin@email.com",
                Password = "password"
            };

            var identityResult1 = await _userManager.CreateAsync(adminUserRequest.ToVkUser(), "password");
            var adminUser = _context.Users.FirstOrDefault(u => u.Email == adminUserRequest.Email);

            var confirmToken1 = await _userManager.GenerateEmailConfirmationTokenAsync(adminUser);
            if (!(confirmToken1.Length > 0)) throw new Exception();
            var confirmed1 = await _userManager.ConfirmEmailAsync(adminUser, confirmToken1);

            if (!confirmed1.Succeeded) throw new Exception("must confirm test user email");

            for (int i = 1; i < 2; i++)
            {
                var org = new Org("A School Where I Teach " + i.ToString(), _config.DefaultStudentLimit);

                org.AddAdmin(adminUser);
                org.AddStudent(adminUser);
                org.AddTeacher(adminUser, _context);

                _context.Organizations.Add(org);
                _context.SaveChanges();

                var tag = await _tagService.GetDefaultTagAsync(org.Id);
                tag.AddUser(adminUser.Id);

                var reading = new Reading("My Reading" +
                    "", "A sentence with a defined word that you can see.");
                var word = new WordEntry("sentence");
                var def1 = new DefinitionBuilder(word.Id);
                def1.AddExampleSentence("A sentence with a defined word that you can see.", reading.Id);
                def1.SetContent("a unit of language", "noun");
                def1.SetSource(DefinitionSourceTypes.UserCode, adminUser.Id);
                word.AddAnnotation(def1.GetAnnotation(), _context);
                _context.Words.Add(word);
                //_context.StudentWords.Add(new StudentWord(
                //    user1.Id, word.Id, word.Annotations.First().Id));
                var ci1 = reading.InsertDefinition(2, 9, word.Id, word.Annotations.First().Id, word.Annotations.First().AnnotationContexts.First().Id, _context);
                word.Annotations.First().AnnotationContexts.First().ContentItemId = ci1.Id;

                var word2 = new WordEntry("defined word");
                var def2 = new DefinitionBuilder(word.Id);
                def2.AddExampleSentence("A sentence with a defined word that you can see.", reading.Id);
                def2.SetContent("word with definition", "noun");
                def2.SetSource(DefinitionSourceTypes.UserCode, adminUser.Id);
                word2.AddAnnotation(def2.GetAnnotation(), _context);
                _context.Words.Add(word2);
                //_context.StudentWords.Add(new StudentWord(
                //user1.Id, word.Id, word.Annotations.First().Id));
                var ci2 = reading.InsertDefinition(18, 29, word2.Id, word2.Annotations.First().Id, word2.Annotations.First().AnnotationContexts.First().Id, _context);
                word2.Annotations.First().AnnotationContexts.First().ContentItemId = ci2.Id;

                reading.AddTag(org.Tags.First());

                org.AddReading(reading, _context);

                for (int j = 0; j < 3; j++)
                {
                    var userRequest = new CreateUserRequest()
                    {
                        FirstName = "First " + j,
                        LastName = "Last " + j,
                        Email = "user" + j + "@email.com",
                        Password = "password"
                    };

                    var umUser = await _userManager.FindByEmailAsync(userRequest.Email);
                    if (umUser == null)
                    {
                        var identityResult = await _userManager.CreateAsync(userRequest.ToVkUser(), "password");
                    }
                    var newUser = _context.Users.FirstOrDefault(u => u.Email == userRequest.Email);

                    var confirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    var confirmed = await _userManager.ConfirmEmailAsync(newUser, confirmToken);

                    org.AddStudent(newUser);
                    tag.AddUser(newUser.Id, _context);
                    // org.AddTeacher(newUser, _context);
                }

                //org.AddTeacherInvite("invite1@teacher.com");
                //org.AddTeacherInvite("invite2@teacher.com");

                //org.AddStudentInvite("invite1@student.com");
                //org.AddStudentInvite("invite2@student.com");

                _context.Organizations.Add(org);
                _context.SaveChanges();


            }
        }
    }
}

