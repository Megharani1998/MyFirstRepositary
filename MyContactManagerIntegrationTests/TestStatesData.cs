using Microsoft.EntityFrameworkCore;
using MyContactManagerData;
using MyContactManagerRepo;
using projectModels;
using Shouldly;

namespace MyContactManagerIntegrationTests
{
    public class TestStatesData
    {

        DbContextOptions<MyContactDBManagerContext> _options;
        private IStateRepositary _repositary;
        private const int NUMBER_OF_STATES = 20;
        private const string IOWA_SPELLING = "Iowa";
        private const string IOWA_MISSPELLING = "Iwoa";

        public TestStatesData()
        {
            SetupOptions();
            BuildDefaults();


        }

        private void BuildDefaults()
        {
            using (var context = new MyContactDBManagerContext(_options))
            {
                var existingStates = Task.Run(() => context.States.ToListAsync()).Result;

                if (existingStates is null || existingStates.Count < 13)
                {
                    var states = GetStatesTestData();
                    context.States.AddRange(states);
                    context.SaveChanges();

                }
            }
        }
        private List<State> GetStatesTestData()
        {
            return new List<State>() {
                 new State() { Id = 1, Name = "Solapur", Abbreviation = "SP" },
                    new State() { Id = 2, Name = "pune", Abbreviation = "PU" },
                    new State() { Id = 3, Name = "akola", Abbreviation = "AK" },
                    new State() { Id = 4, Name = "kurnul", Abbreviation = "KR" },
                    new State() { Id = 5, Name = "maipal", Abbreviation = "MP" },
                    new State() { Id = 6, Name = "UttarPradesh", Abbreviation = "UP" },
                    new State () { Id = 7, Name = "Maharashtra", Abbreviation = "MH" },
                    new State() { Id = 8, Name = "gujrat", Abbreviation = "GR" },
                    new State() { Id = 9, Name = "Bangal", Abbreviation = "BG" },
                    new State() { Id = 10, Name = "Rajsthan", Abbreviation = "RS" },
                    new State() { Id = 11, Name = "Manipur", Abbreviation = "MPR" },
                    new State() { Id = 12, Name = "Ratnagiri", Abbreviation = "RG" },
                    new State() { Id = 13, Name = "Goa", Abbreviation = "GO" },
                    new State() { Id = 14, Name = "ABCD", Abbreviation = "AB" },
                    new State() { Id = 15, Name = "efgh", Abbreviation = "EF" },
                    new State() { Id = 16, Name = "IWOA", Abbreviation = "IJ" },
                    new State() { Id = 17, Name = "MNOP", Abbreviation = "MN" },
                    new State() { Id = 18, Name = "QRST", Abbreviation = "QR" },
                    new State() { Id = 19, Name = "UVWX", Abbreviation = "UV" },
                    new State() { Id = 20, Name = "YZAB", Abbreviation = "YZ" }

            };
        }

        private void SetupOptions()
        {
            _options = new DbContextOptionsBuilder<MyContactDBManagerContext>().UseInMemoryDatabase(databaseName: "MyContactManagerStatesTests").
                Options;

        }

        [Theory]


        [InlineData("akola", "AK", 2)]
        [InlineData("YZAB", "YZ", 19)]
        [InlineData("UVWX", "UV", 18)]
        [InlineData("Bangal", "BG", 8)]

        public async void TestGetAllState(string name, string abbreviation, int index)
        {
            using (var context = new MyContactDBManagerContext(_options))
            {
                _repositary = new StatesRepositary(context);
                var states = await _repositary.GetAllAsync();
                states.Count.ShouldBe(NUMBER_OF_STATES);
                states[index].Name.ShouldBe(name, StringCompareShould.IgnoreCase);
                states[index].Abbreviation.ShouldBe(abbreviation, StringCompareShould.IgnoreCase);
            }

        }
        [Theory]

        [InlineData("akola", "AK", 3)]
        [InlineData("YZAB", "YZ", 20)]
        [InlineData("UVWX", "UV", 19)]
        [InlineData("Bangal", "BG", 9)]

        private async void TestGetAState(string name, string abbreviation, int stateId)
        {

            using (var context = new MyContactDBManagerContext(_options))
            {
                _repositary = new StatesRepositary(context);

                var state = await _repositary.GetAsync(stateId);
                state.ShouldNotBeNull();
                state.Name.ShouldBe(name, StringCompareShould.IgnoreCase);
                state.Abbreviation.ShouldBe(abbreviation, StringCompareShould.IgnoreCase);
            }
        }
        [Fact]
        public async Task UpdateState()
        {

            using (var context = new MyContactDBManagerContext(_options))
            {
                _repositary = new StatesRepositary(context);
                var stateToUpdate = await _repositary.GetAsync(16);
                stateToUpdate.ShouldNotBeNull();
                stateToUpdate.Name.ShouldBe(IOWA_MISSPELLING, StringCompareShould.IgnoreCase);

                stateToUpdate.Name = IOWA_SPELLING;
                await _repositary.AddOrUpdateAsync(stateToUpdate);

                var updatedState = await _repositary.GetAsync(16);
                updatedState.ShouldNotBeNull();
                updatedState.Name.ShouldBe(IOWA_SPELLING, StringCompareShould.IgnoreCase);

                //put it back
                updatedState.Name = "iwoa";
                await _repositary.AddOrUpdateAsync(updatedState);

                var revertedState = await _repositary.GetAsync(16);
                revertedState.ShouldNotBeNull();
                revertedState.Name.ShouldBe(IOWA_MISSPELLING, StringCompareShould.IgnoreCase);

            }

        }

        [Fact]
        public async Task AddAndDeleteState()
        {
            using (var context = new MyContactDBManagerContext(_options))
            {
                _repositary = new StatesRepositary(context);

                //add a state and validate it is stored
                var stateToAdd = new State() { Id = 0, Name = "star Trek - The Next Generation", Abbreviation = "TNG" };
                await _repositary.AddOrUpdateAsync(stateToAdd);

                var updatedState = await _repositary.GetAsync(stateToAdd.Id);
                updatedState.ShouldNotBeNull();
                updatedState.Name.ShouldBe("Star Trek - The Next Generation", StringCompareShould.IgnoreCase);
                updatedState.Abbreviation.ShouldBe("TNG", StringCompareShould.IgnoreCase);

                //delete to keep current count and list in tact.
                await _repositary.DeleteAsync(updatedState.Id);
                var deletedState = await _repositary.GetAsync(updatedState.Id);
                deletedState.ShouldBeNull();


            }

        }
    }
}