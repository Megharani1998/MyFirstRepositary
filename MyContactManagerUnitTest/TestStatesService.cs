
using Moq;
using MyContactManagerRepo;
using MyContactsManagerServices;
using projectModels;
using Shouldly;

namespace MyContactManagerUnitTest
{
    public class TestStatesSerivce
    {
        private IStateService _statesService;
        private Mock<IStateRepositary> _repositary;
        private int NUMBER_OF_STATES = 20;

        public TestStatesSerivce()
        {
            CreateMocks();
            _statesService = new StatesService(_repositary.Object);

        }

        private void CreateMocks()
        {
            _repositary = new Mock<IStateRepositary>();
            var states = GetStatesTestData();
            var singleState = GetSingleState();

            _repositary.Setup(x=>x.GetAllAsync()).Returns(states);
            _repositary.Setup(x=>x.GetAsync(It.IsAny<int>())).Returns(singleState);
        }

        private async Task<State> GetSingleState()
        {
            return new State() { Id = 1, Name = "Solapur", Abbreviation = "SP" };

        }

        private async Task<IList<State>> GetStatesTestData()
        {
            return new List<State>() {
                 new State() { Id = 1, Name = "Solapur", Abbreviation = "SP" },
                    new State() { Id = 2, Name = "pune", Abbreviation = "PU" },
                    new State() { Id = 3, Name = "akola", Abbreviation = "AK" },
                    new State() { Id = 4, Name = "kurnul", Abbreviation = "KR" },
                    new State() { Id = 5, Name = "maipal", Abbreviation = "MP" },
                    new State() { Id = 6, Name = "UttarPradesh", Abbreviation = "UP" },
                    new State() { Id = 7, Name = "Maharashtra", Abbreviation = "MH" },
                    new State() { Id = 8, Name = "gujrat", Abbreviation = "GR" },
                    new State() { Id = 9, Name = "Bangal", Abbreviation = "BG" },
                    new State() { Id = 10, Name = "Rajsthan", Abbreviation = "RS" },
                    new State() { Id = 11, Name = "Manipur", Abbreviation = "MPR" },
                    new State() { Id = 12, Name = "Ratnagiri", Abbreviation = "RG" },
                    new State() { Id = 13, Name = "Goa", Abbreviation = "GO" },
                    new State() { Id = 14, Name = "ABCD", Abbreviation = "AB" },
                    new State() { Id = 15, Name = "efgh", Abbreviation = "EF" },
                    new State() { Id = 16, Name = "Ijkl", Abbreviation = "IJ" },
                    new State() { Id = 17, Name = "MNOP", Abbreviation = "MN" },
                    new State() { Id = 18, Name = "QRST", Abbreviation = "QR" },
                    new State() { Id = 19, Name = "UVWX", Abbreviation = "UV" },
                    new State() { Id = 20, Name = "YZAB", Abbreviation = "YZ" }

            };
        }
      
        [Theory]

        [InlineData("akola", "AK", 1)]
        [InlineData("YZAB", "YZ", 19)]
        [InlineData("UVWX", "UV", 18)]
        [InlineData("Bangal", "BG", 2)]





        public async void TestGetAllState(string name, string abbreviation, int index)
        {
            //test case get passed if data of states index is ordred in asechending order
            var states = await _statesService.GetAllAsync();
            states.ShouldNotBeNull();
            states.Count.ShouldBe(NUMBER_OF_STATES);
            states[index].Name.ShouldBe(name, StringCompareShould.IgnoreCase);
            states[index].Abbreviation.ShouldBe(abbreviation, StringCompareShould.IgnoreCase);
            
        }
        [Fact]
        private async void TestGetAState()
        {
            var States = await _statesService.GetAsync(23523);
            States.ShouldNotBeNull();
            States.Name.ShouldBe("Solapur", StringCompareShould.IgnoreCase);
            States.Abbreviation.ShouldBe("SP", StringCompareShould.IgnoreCase);
        }

    }
     
    }
