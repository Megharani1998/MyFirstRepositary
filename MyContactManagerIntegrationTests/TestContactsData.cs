using Microsoft.EntityFrameworkCore;

using MyContactManagerData;
using MyContactManagerRepo;
using projectModels;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyContactManagerIntegrationTests
{
    public class TestContactsData
    {
        DbContextOptions<MyContactDBManagerContext> _options;
        private IContactsRepositary _repositary;

        private const int NUMBER_OF_CONTACTS = 5;
        private const string USERID1 = "466f8125";
        private const string USERID2 = "466f8126";
        private const string USERID3 = "466f8127";
        private const string FIRST_NAME_1 = "Megha";
        private const string LAST_NAME_1 = "Maske";
        private const string EMAIL_1 = "maske@12";
        private const string FIRST_NAME_2 = "chetana";
        private const string LAST_NAME_2 = "chadhari";
        private const string EMAIL_2 = "chetana@12";
        private const string FIRST_NAME_3 = "kajal";
        private const string LAST_NAME_3 = "lote";
        private const string EMAIL_3 = "kajal@12";
        private const string FIRST_NAME_4 = "shaunak";
        private const string LAST_NAME_4 = "kulkarni";
        private const string EMAIL_4 = "shaunak@12";
        private const string FIRST_NAME_5 = "Mahi";
        private const string LAST_NAME_5 = "Maske";
        private const string EMAIL_5 = "mahi@12";
        private const string FIRST_NAME_5_UPDATED = "Mahendra";

        public TestContactsData()
        {
            SetUpOptions();
            BuildDefaults();
        }
        private void SetUpOptions() {

            _options = new DbContextOptionsBuilder<MyContactDBManagerContext>().
                UseInMemoryDatabase(databaseName: "MyContactManagerContactsTests").
                Options;
        }
        public void BuildDefaults()
        {
            using (var context = new MyContactDBManagerContext(_options))
            {

                var existingState = Task.Run(() => context.States.ToListAsync()).Result;
                if (existingState is null || existingState.Count < 5)
                {
                    var states = GetStatesTestData();
                    context.States.AddRange(states);
                    context.SaveChanges();
                }
                var existingContacts = Task.Run(() => context.Contacts.ToListAsync()).Result;
                if (existingContacts is null || existingContacts.Count < 5)
                {
                    var contacts = GetContactsTestData();
                    context.Contacts.AddRange(contacts.ToList());
                    context.Contacts.AddRange(contacts);

                    context.SaveChanges();
                }
            }
        }
        
        private List<Contacts> GetContactsTestData()
           
        {
            return new List<Contacts>()
            {
                new Contacts() {Id = 1, BirthDate= new DateTime(1998,1,1),City = "Los Angles", Email = EMAIL_1,
                FirstName= FIRST_NAME_1, LastName = LAST_NAME_1,PhonePrimary = "7775005894",PhoneSecondary="9898989898",
                UserId=USERID1, StreetAddress1 ="444 fourth", StreetAddress2 ="streetno 2",Zip = "1111"},

                new Contacts() { Id = 2, BirthDate = new DateTime(1999, 2, 1), City = "calforn", Email = EMAIL_2,
                FirstName = FIRST_NAME_2, LastName = LAST_NAME_2, PhonePrimary = "8775005894", PhoneSecondary = "8998989898",
                UserId = USERID1, StreetAddress1 = "555 fourth", StreetAddress2 = "streetno 3", Zip = "11121" },


                new Contacts() {Id = 3, BirthDate= new DateTime(1968,4,1),City = "Loss", Email = EMAIL_3,
                FirstName= FIRST_NAME_3, LastName = LAST_NAME_3,PhonePrimary = "9875005894",PhoneSecondary="9850989898",
                UserId=USERID2, StreetAddress1 ="999 fourth", StreetAddress2 ="streetno 9",Zip = "12121"},

                new Contacts() {Id = 4, BirthDate= new DateTime(1988,1,7),City = "Angles", Email = EMAIL_4,
                FirstName= FIRST_NAME_4, LastName = LAST_NAME_4,PhonePrimary = "7775006767",PhoneSecondary="9898956898",
                UserId=USERID2, StreetAddress1 ="984 fourth", StreetAddress2 ="streetno 8",Zip = "14311"},


                new Contacts() {Id = 5, BirthDate= new DateTime(1998,1,1),City = "Lossles", Email = EMAIL_5,
                FirstName= FIRST_NAME_5, LastName = LAST_NAME_5,PhonePrimary = "7775034894",PhoneSecondary="9548989898",
                UserId=USERID3, StreetAddress1 ="123 fourth", StreetAddress2 ="streetno 2",Zip = "1098"}

            };
        }
        private List<State> GetStatesTestData()
        {
            return new List<State>(){
               new State() { Id = 1, Name = "Alabama", Abbreviation = "AL" },
         new State() { Id = 2, Name = "Alaska", Abbreviation = "AK" },
          new State() { Id = 3, Name = "Arizona", Abbreviation = "AZ" },
           new State() { Id = 4, Name = "Arcansas ", Abbreviation = "AR" },
           new State() { Id = 5, Name = "California", Abbreviation = "CA" }
        };
        }
        [Theory]


        [InlineData(FIRST_NAME_1, LAST_NAME_1, EMAIL_1, USERID1, 1, 2)]
        [InlineData(FIRST_NAME_2, LAST_NAME_2, EMAIL_2, USERID1, 0, 2)]
        [InlineData(FIRST_NAME_3, LAST_NAME_3, EMAIL_3, USERID2, 1, 2)]
        [InlineData(FIRST_NAME_4, LAST_NAME_4, EMAIL_4, USERID2, 0, 2)]
        [InlineData(FIRST_NAME_5, LAST_NAME_5, EMAIL_5, USERID3, 0, 1)]
        public async Task TestGetAllContacts(string firstName, string lastName, string email, string userId, int index,int expectedCount)
        {

            using (var context = new MyContactDBManagerContext(_options))
            {
                //_repositary = new ContactsRepositary(context);
                _repositary = new ContactsRepositary(context);
                var contacts = await _repositary.GetAllAsync(userId);
                contacts.Count.ShouldBe(expectedCount);
                contacts[index].FirstName.ShouldBe(firstName, StringCompareShould.IgnoreCase);
                contacts[index].LastName.ShouldBe(lastName, StringCompareShould.IgnoreCase);
                contacts[index].Email.ShouldBe(email, StringCompareShould.IgnoreCase);
                contacts[index].UserId.ShouldBe(userId, StringCompareShould.IgnoreCase);

            }
        }
        [Theory]
        [InlineData(FIRST_NAME_1, LAST_NAME_1, EMAIL_1, USERID1, 1)]
        [InlineData(FIRST_NAME_2, LAST_NAME_2, EMAIL_2, USERID1, 2)]
        [InlineData(FIRST_NAME_3, LAST_NAME_3, EMAIL_3, USERID2, 3)]
        [InlineData(FIRST_NAME_4, LAST_NAME_4, EMAIL_4, USERID2, 4)]
        [InlineData(FIRST_NAME_5, LAST_NAME_5, EMAIL_5, USERID3, 5)]
        public async Task GetContact(string firstName, string lastName, string email, string userId, int contactId) {

            using (var context = new MyContactDBManagerContext(_options))
            {
                _repositary = new ContactsRepositary(context);
                var contacts = await _repositary.GetAsync(contactId,userId);
                contacts.ShouldNotBeNull();
                contacts.FirstName.ShouldBe(firstName, StringCompareShould.IgnoreCase);
                contacts.LastName.ShouldBe(lastName, StringCompareShould.IgnoreCase);
                contacts.Email.ShouldBe(email, StringCompareShould.IgnoreCase);
                contacts.UserId.ShouldBe(userId, StringCompareShould.IgnoreCase);


            }
        }
        [Theory]
        [InlineData(FIRST_NAME_1, LAST_NAME_1, EMAIL_1, USERID1, 1)]
        [InlineData(FIRST_NAME_2, LAST_NAME_2, EMAIL_2, USERID1, 2)]
        [InlineData(FIRST_NAME_3, LAST_NAME_3, EMAIL_3, USERID2, 3)]
        [InlineData(FIRST_NAME_4, LAST_NAME_4, EMAIL_4, USERID2, 4)]
        [InlineData(FIRST_NAME_5, LAST_NAME_5, EMAIL_5, USERID3, 5)]
        public async Task canNotGetSomeoneElseContacts(string firstName, string lastName, string email, string userId, int contactId)
        {

            using (var context = new MyContactDBManagerContext(_options))
            {
                _repositary = new ContactsRepositary(context);
                var contact = await _repositary.GetAsync(contactId, userId);
                contact.ShouldNotBeNull();

            }
        }
        [Fact]
        public async Task UpdateContact() {

            using (var context = new MyContactDBManagerContext(_options))
            {
                _repositary = new ContactsRepositary(context);
                var contactToUpdate = await _repositary.GetAsync(5,USERID3);
                contactToUpdate.ShouldNotBeNull();
                contactToUpdate.FirstName.ShouldBe(FIRST_NAME_5, StringCompareShould.IgnoreCase);

                contactToUpdate.FirstName = FIRST_NAME_5_UPDATED;
                await _repositary.AddOrUpdateAsync(contactToUpdate, USERID3);

                var updatedContact = await _repositary.GetAsync(5,USERID3);
                updatedContact.ShouldNotBe(null);
                updatedContact.FirstName.ShouldBe(FIRST_NAME_5_UPDATED, StringCompareShould.IgnoreCase);

                //put it back
                updatedContact.FirstName = FIRST_NAME_5;
                await _repositary.AddOrUpdateAsync(updatedContact, USERID3);

                var revertedState = await _repositary.GetAsync(5,USERID3);
                revertedState.ShouldNotBe(null);
                revertedState.FirstName.ShouldBe(FIRST_NAME_5, StringCompareShould.IgnoreCase);

            } }
        [Fact]
        public async Task AddAndDeleteContact()
        {

            using (var context = new MyContactDBManagerContext(_options))
            {
                _repositary = new ContactsRepositary(context);

                var contactToAdd = new Contacts()
                {
                    BirthDate = new DateTime(1989, 1, 1),
                    City = "somewhere",
                    Email = "ma@com",
                    FirstName = "Measf",
                    LastName = "acvf",
                    PhonePrimary = "1234567890",
                    PhoneSecondary = "1234567856",
                    StateId = 4,
                    StreetAddress1 = "123 fourth",
                    StreetAddress2 = "123 fdgsf",
                    UserId = USERID2,
                    Zip = "12111"
                };
                await _repositary.AddOrUpdateAsync(contactToAdd, USERID2);
                var updatedContact = await _repositary.GetAsync(contactToAdd.Id, USERID2);
                updatedContact.ShouldNotBe(null);
                updatedContact.FirstName.ShouldBe("Measf", StringCompareShould.IgnoreCase);
                updatedContact.Email.ShouldBe("ma@com", StringCompareShould.IgnoreCase);

                //delete to keep current cound and list in tact
                await _repositary.DeleteAsync(updatedContact.Id, USERID2);
                var deletedCont = await _repositary.GetAsync(updatedContact.Id, USERID2);
                deletedCont.ShouldBe(null);
                }
            }
        }
    }
