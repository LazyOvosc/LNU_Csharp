#pragma warning disable CS8600
using System.Threading.Channels;

class Astronaut
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public int BirthYear { get; set; }

    public Astronaut(string name, string surname, int birthYear)
    {
        Name = name;
        Surname = surname;
        BirthYear = birthYear;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Astronaut other = (Astronaut)obj;
        return Name == other.Name && Surname == other.Surname && BirthYear == other.BirthYear;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + Name.GetHashCode();
            hash = hash * 23 + Surname.GetHashCode();
            hash = hash * 23 + BirthYear.GetHashCode();
            return hash;
        }
    }
}

class SpaceShip
{
    public string Title { get; set; }
    public int YearOfManufacture { get; set; }

    public SpaceShip(string title, int yearOfManufacture)
    {
        Title = title;
        YearOfManufacture = yearOfManufacture;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        SpaceShip other = (SpaceShip)obj;
        return Title == other.Title && YearOfManufacture == other.YearOfManufacture;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + Title.GetHashCode();
            hash = hash * 23 + YearOfManufacture.GetHashCode();
            return hash;
        }
    }
}

class Mission
{
    public string Title { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public List<Astronaut> Crew { get; set; }
    public SpaceShip SpaceShip { get; set; }

    public Mission(string title, DateTime start, DateTime end, List<Astronaut> crew, SpaceShip spaceShip)
    {
        Title = title;
        Start = start;
        End = end;
        Crew = crew;
        SpaceShip = spaceShip;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Mission other = (Mission)obj;
        return Title == other.Title && Start == other.Start && End == other.End
            && Crew.SequenceEqual(other.Crew) && SpaceShip.Equals(other.SpaceShip);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + Title.GetHashCode();
            hash = hash * 23 + Start.GetHashCode();
            hash = hash * 23 + End.GetHashCode();
            hash = hash * 23 + Crew.GetHashCode();
            hash = hash * 23 + SpaceShip.GetHashCode();
            return hash;
        }
    }
    public override string ToString()
    {
        string astronautNames = string.Join(", ", Crew.Select(astronaut => astronaut.Name));
        return $"Title: {Title}, Astronauts: {astronautNames}, Start: {Start}, End: {End}, SpaceShip: {SpaceShip.Title}";
    }
}
static class LINQHandler
{
    public static void TotalMissionsTime(List<Mission> missions)
    {
        int totalTime = (int)(from mission in missions
                         select (mission.End - mission.Start).TotalDays)
                         .Sum();
        Console.WriteLine($"Total Time of all missions is: {totalTime}");
        Console.WriteLine();
    }
    public static void LongestMission(List<Mission> missions)
    {
        Mission longestMission = (from mission in missions
                                  orderby (mission.End - mission.Start).TotalDays descending
                                  select mission)
                                  .FirstOrDefault();
        Console.WriteLine($"Longest mission: {longestMission}");
        Console.WriteLine();
    }
    public static void AstronautsMissionsAndTotalDuration(List<Mission> missions)
    {
        var astronauts = (from mission in missions
                          from astronaut in mission.Crew
                          select astronaut).Distinct();

        foreach (var astronaut in astronauts)
        {
            var astronautMissions = (from mission in missions
                                     where mission.Crew.Contains(astronaut)
                                     select mission).ToList();

            double totalDuration = (from mission in astronautMissions
                                    select (mission.End - mission.Start).TotalDays)
                                    .Sum();

            Console.WriteLine($"Astronaut: {astronaut.Name}");
            Console.WriteLine("Missions:");
            foreach (var mission in astronautMissions)
            {
                Console.WriteLine(mission);
            }
            Console.WriteLine($"Total Duration in Space: {(int)totalDuration} days");
            Console.WriteLine();
        }
        Console.WriteLine();
    }
    public static void LongestMissionBySpaceShip(List<Mission> missions)
    {
        var longestMissionSpaceShip = (from mission in missions
                                       group mission by mission.SpaceShip into spaceshipGroup
                                       let totalDuration = spaceshipGroup.Sum(m => (m.End - m.Start).TotalDays)
                                       orderby totalDuration descending
                                       select new
                                       {
                                           SpaceShip = spaceshipGroup.Key
                                       }).FirstOrDefault();

        var longestMissionSpaceShipMissions = (from mission in missions
                                                where mission.SpaceShip == longestMissionSpaceShip.SpaceShip
                                                orderby (mission.End - mission.Start).TotalDays
                                                select mission).ToList();

        Console.WriteLine($"SpaceShip: {longestMissionSpaceShip.SpaceShip.Title}");
        Console.WriteLine("Missions:");
        foreach (var mission in longestMissionSpaceShipMissions)
        {
            Console.WriteLine(mission);
        }
        Console.WriteLine();
        
    }
    public static void AstronautsByTimePeriod(List<Mission> missions, int startYear, int endYear)
    {
        var astronauts = (from mission in missions
                          from astronaut in mission.Crew
                          where mission.Start.Year >= startYear && mission.End.Year <= endYear
                          select astronaut).Distinct();

        Console.WriteLine($"Astronauts who had missions between {startYear} and {endYear}:");

        foreach (var astronaut in astronauts)
        {
            Console.WriteLine($"{astronaut.Name} {astronaut.Surname}");
        }
        Console.WriteLine();
    }
    public static void SpaceShipWithLongestOperation(List<Mission> missions)
    {
        var spaceShipWithLongestOperation = (from mission in missions
                                             group mission by mission.SpaceShip into spaceshipGroup
                                             let lastMission = spaceshipGroup.OrderByDescending(m => m.End).FirstOrDefault()
                                             let operationYears = lastMission.End.Year - lastMission.SpaceShip.YearOfManufacture
                                             orderby operationYears descending
                                             select new
                                             {
                                                 SpaceShip = spaceshipGroup.Key
                                             }).FirstOrDefault();

        if (spaceShipWithLongestOperation != null)
        {
            Console.WriteLine($"SpaceShip with Longest Operation:");
            Console.WriteLine($"Title: {spaceShipWithLongestOperation.SpaceShip.Title}");
        }
        Console.WriteLine();
    }
}

class Program
{
    public static void Main(string[] args)
    {
        
        List<Mission> missions = new List<Mission>();
        #region MissionsMaking
        Astronaut astronaut1 = new Astronaut("John", "Doe", 1985);
        Astronaut astronaut2 = new Astronaut("Jane", "Smith", 1990);
        Astronaut astronaut3 = new Astronaut("Michael", "Johnson", 1988);

        SpaceShip spaceship1 = new SpaceShip("Galaxy Cruiser", 2020);
        SpaceShip spaceship2 = new SpaceShip("Stellar Voyager", 2015);

        Mission mission1 = new Mission("Mission 1", DateTime.Now.AddDays(-1020), DateTime.Now.AddDays(-1010),
            new List<Astronaut> { astronaut1, astronaut2 }, spaceship1);

        Mission mission2 = new Mission("Mission 2", DateTime.Now.AddDays(-10030), DateTime.Now.AddDays(-10023),
            new List<Astronaut> { astronaut2, astronaut3 }, spaceship2);

        Mission mission3 = new Mission("Mission 3", DateTime.Now.AddDays(-5021), DateTime.Now.AddDays(-5016),
            new List<Astronaut> { astronaut1, astronaut3 }, spaceship1);

        Mission mission4 = new Mission("Mission 4", DateTime.Now.AddDays(-17033), DateTime.Now.AddDays(-17030),
            new List<Astronaut> { astronaut2, astronaut3 }, spaceship2);

        Mission mission5 = new Mission("Mission 5", DateTime.Now.AddDays(-14040), DateTime.Now.AddDays(-14039),
            new List<Astronaut> { astronaut3 }, spaceship1);

        missions.Add(mission1);
        missions.Add(mission2);
        missions.Add(mission3);
        missions.Add(mission4);
        missions.Add(mission5);
        #endregion MissionsMaking
        
        Console.ForegroundColor = ConsoleColor.Cyan;
        LINQHandler.TotalMissionsTime(missions);
        Console.ForegroundColor = ConsoleColor.Yellow;
        LINQHandler.LongestMission(missions);
        Console.ForegroundColor = ConsoleColor.Cyan;
        LINQHandler.AstronautsMissionsAndTotalDuration(missions);
        Console.ForegroundColor = ConsoleColor.Yellow;
        LINQHandler.LongestMissionBySpaceShip(missions);
        Console.ForegroundColor = ConsoleColor.Cyan;
        LINQHandler.AstronautsByTimePeriod(missions, 2000, 2007);
        Console.ForegroundColor = ConsoleColor.Yellow;
        LINQHandler.SpaceShipWithLongestOperation(missions);
        Console.ResetColor();
    }
}