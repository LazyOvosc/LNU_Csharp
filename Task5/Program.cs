using System;
using System.Collections.Generic;

namespace IndividualTask5
{
    class Table<R, C, V>
    {
        private Dictionary<Tuple<R, C>, V> data = new Dictionary<Tuple<R, C>, V>();

        public V this[R row, C column]
        {
            get
            {
                return data[Tuple.Create(row, column)];
            }
            set
            {
                data[Tuple.Create(row, column)] = value;
            }
        }

        public override string ToString()
        {
            string result = "";
            foreach (var row in data.GroupBy(item => item.Key.Item1))
            {
                result += $"{row.Key.ToString()}:\n";
                foreach (var cell in row)
                {
                    result += $"\t{cell.Key.Item2.ToString()} - ";
                    if (cell.Value is HashSet<int> hashset)
                    {
                        result += $"{string.Join(", ", hashset)}\n";
                    }
                    else
                    {
                        result += $"{cell.Value.ToString()}\n";
                    }
                }
            }
            return result;
        }
    }

    class FootballTeam
    {
        public string Name { get; set; }
        public string City { get; set; }
        public int FoundationYear { get; set; }

        public FootballTeam(string name, string city, int foundationYear)
        {
            Name = name;
            City = city;
            FoundationYear = foundationYear;
        }

        public override string ToString()
        {
            string final_team = Name + " " + City + " " + FoundationYear;
            return final_team;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            FootballTeam other = (FootballTeam)obj;

            return Name == other.Name &&
                   City == other.City &&
                   FoundationYear == other.FoundationYear;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ City.GetHashCode() ^ FoundationYear.GetHashCode();
        }
    }

    class Tournament
    {
        public string Name { get; set; }
        public bool International { get; set; }
        public int FoundationYear { get; set; }

        public Tournament(string name, bool international, int foundationYear)
        {
            Name = name;
            International = international;
            FoundationYear = foundationYear;
        }

        public override string ToString()
        {
            string final_tournament = Name + " " + International + " " + FoundationYear;
            return final_tournament;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Tournament other = (Tournament)obj;

            return Name == other.Name &&
                   International == other.International &&
                   FoundationYear == other.FoundationYear;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ International.GetHashCode() ^ FoundationYear.GetHashCode();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Table<FootballTeam, Tournament, HashSet<int>> table = new Table<FootballTeam, Tournament, HashSet<int>>();

            FootballTeam team1 = new FootballTeam("Real Madrid", "Madrid", 1902);
            FootballTeam team2 = new FootballTeam("Barcelona", "Barcelona", 1899);
            FootballTeam team3 = new FootballTeam("Liverpool", "Liverpool", 1892);
            FootballTeam team11 = new FootballTeam("Real Madrid", "Madrid", 1902);

            Tournament tournament1 = new Tournament("La Liga", false, 1929);
            Tournament tournament2 = new Tournament("Champions League", true, 1955);

            HashSet<int> years1 = new HashSet<int>() { 1958, 1959, 1960, 1998, 2000, 2002 };
            HashSet<int> years2 = new HashSet<int>() { 1992, 2006, 2009, 2011, 2015 };
            HashSet<int> years3 = new HashSet<int>() { 1955, 1990, 1993 };
            table[team1, tournament1] = years1;
            table[team2, tournament2] = years2;
            table[team3, tournament1] = years3;

            HashSet<int> years = table[team11, tournament1];

            Console.WriteLine(table);
        }
    }
}


