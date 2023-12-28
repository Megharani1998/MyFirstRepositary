using Mono.TextTemplating;
using Moq;
using MyContactManagerData;
using MyContactManagerRepo;
using MyContactsManagerServices;
using projectModels;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyContactManagerUnitTest
{
    public class TestContactsService
    {
        private IContactsSerivce _contactsSerivce;
        private Mock<IContactsRepositary> _contactsRepositary;

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

        public TestContactsService()
        {

            CreateMocks();
            _contactsSerivce = new ContactsService(_contactsRepositary.Object);
        }
        private void CreateMocks()
        {
            _contactsRepositary = new Mock<IContactsRepositary>();
            var contacts = GetContactsTestData();
            var singleContact = GetSingleContact();

            _contactsRepositary.Setup(x => x.GetAllAsync(It.IsAny<string>())).Returns(contacts);
            _contactsRepositary.Setup(x => x.GetAsync(It.IsAny<int>(),It.IsAny<string>())).Returns(singleContact);
        }

        private async Task<Contacts?> GetSingleContact()
        {
            return new Contacts()
            {
                Id = 4,
                BirthDate = new DateTime(1989, 1, 1),
                City = "somewhere",
                Email = EMAIL_4,
                FirstName = FIRST_NAME_4,
                LastName = LAST_NAME_4,
                PhonePrimary = "1234567890",
                PhoneSecondary = "1234567856",
                StateId = 2,
                StreetAddress1 = "123 fourth",
                StreetAddress2 = "123 fdgsf",
                UserId = USERID2,
                Zip = "12111"
            };
        }

        private async Task<IList<Contacts>> GetContactsTestData()
        {
            return new List<Contacts>() {
            new Contacts() {Id = 1, BirthDate= new DateTime(1998,1,1),City = "Los Angles", Email = EMAIL_1,
                FirstName= FIRST_NAME_1, LastName = LAST_NAME_1,PhonePrimary = "7775005894",PhoneSecondary="9898989898",
                UserId=USERID1, StreetAddress1 ="444 fourth", StreetAddress2 ="streetno 2",Zip = "1111"},

                new Contacts() {Id = 2, BirthDate= new DateTime(1999,2,1),City = "calforn", Email = EMAIL_2,
                FirstName= FIRST_NAME_2, LastName = LAST_NAME_2,PhonePrimary = "8775005894",PhoneSecondary="8998989898",
                UserId=USERID1, StreetAddress1 ="555 fourth", StreetAddress2 ="streetno 3",Zip = "11121"},


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
        [Theory]
        [InlineData(FIRST_NAME_1, LAST_NAME_1, EMAIL_1, USERID1, 0)]
        [InlineData(FIRST_NAME_2, LAST_NAME_2, EMAIL_2, USERID1, 1)]
        [InlineData(FIRST_NAME_3, LAST_NAME_3, EMAIL_3, USERID2, 2)]
        [InlineData(FIRST_NAME_4, LAST_NAME_4, EMAIL_4, USERID2, 3)]
        [InlineData(FIRST_NAME_5, LAST_NAME_5, EMAIL_5, USERID3, 4)]
        public async Task TestGetAllContacts(string firstName, string lastName, string email, string userId, int index)
        {
            var contacts = await _contactsSerivce.GetAllAsync(USERID1);
            contacts.Count.ShouldBe(NUMBER_OF_CONTACTS);
            contacts[index].FirstName.ShouldBe(firstName, StringCompareShould.IgnoreCase);
            contacts[index].LastName.ShouldBe(lastName, StringCompareShould.IgnoreCase);
            contacts[index].Email.ShouldBe(email, StringCompareShould.IgnoreCase);
            contacts[index].UserId.ShouldBe(userId, StringCompareShould.IgnoreCase);


        }
        [Fact]
        public async Task TestGetAContact()
        {
            var contacts = await _contactsSerivce.GetAsync(123,"asdf");
            contacts.ShouldNotBeNull();
            contacts.FirstName.ShouldBe(FIRST_NAME_4, StringCompareShould.IgnoreCase);
            contacts.LastName.ShouldBe(LAST_NAME_4, StringCompareShould.IgnoreCase);
            contacts.Email.ShouldBe(EMAIL_4, StringCompareShould.IgnoreCase);
            contacts.UserId.ShouldBe(USERID2, StringCompareShould.IgnoreCase);
            contacts.StateId.ShouldBe(2);


        }
    }
}
