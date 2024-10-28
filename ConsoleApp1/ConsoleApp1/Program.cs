using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Xml.Linq;

public class FullTest
{
    public static void Main(string[] args)
    {
        Voting.Action();
    }

    enum Gender
    {
        Male,
        Female
    }
    class Voter
    {
        internal string Name;
        internal Gender Gender;
        internal int Age;
        internal string? VoterChoice;

        internal Voter(string Name, Gender Gender, int Age, string? VoterChoice)
        {
            this.Name = Name;
            this.Gender = Gender;
            this.Age = Age;
            this.VoterChoice = VoterChoice;
        }
    }

    class Voters
    {
        internal static List<Voter> voters = new List<Voter>
        {
            new Voter(Name: "Arthur", Gender: Gender.Male, Age: 28, VoterChoice: "Bush"),
            new Voter(Name: "Levon", Gender: Gender.Male, Age: 32, VoterChoice: null),
            new Voter(Name: "Luiza", Gender: Gender.Female, Age: 28, VoterChoice: "Clinton"),
            new Voter(Name: "Anna", Gender: Gender.Female, Age: 25, VoterChoice: null),
            new Voter(Name: "David", Gender: Gender.Male, Age: 50, VoterChoice: null),
            new Voter(Name: "Sophia", Gender: Gender.Female, Age: 22, VoterChoice: null),
            new Voter(Name: "John", Gender: Gender.Male, Age: 37, VoterChoice: null),
            new Voter(Name: "Marta", Gender: Gender.Female, Age: 29, VoterChoice: null),
            new Voter(Name: "Chris", Gender: Gender.Male, Age: 31, VoterChoice: null),
            new Voter(Name: "Olivia", Gender: Gender.Female, Age: 27, VoterChoice: null)
        };
    }

    class Candidates
    {

        internal static Dictionary<string, int> candidates = new Dictionary<string, int>()
        {
            { "Bush", 0},
            { "Clinton", 0},
            { "Trump", 0},
        };

        internal static List<string> keys = new List<string>(candidates.Keys);
    }

    class Voting
    {

        internal static int WrongVotes = 0;
        private static bool IsAlreadyCounted = false;
        internal static string CallOfUserInput()
        {
            return Console.ReadLine();
        }

        internal static void Action()
        {
            string action = "";

            while (action != "5")
            {
                Console.WriteLine("_______________________");
                Console.WriteLine("What to do? You can 1.Vote, 2.Know voting details, 3. Know Votes by candidates, 4.Add a new voter to the list, 5. Quit");
                action = CallOfUserInput();

                switch (action)
                {
                    case "1":
                        Vote();
                        break;
                    case "2":
                        PrintVotingDetails();
                        break;
                    case "3":
                        PrintCandidateVotes();
                        break;
                    case "4":
                        AddVoter();
                        break;
                    case "5":
                        Console.WriteLine("Exiting...");
                        break;
                    default:
                        Console.WriteLine("Wrong input, Try again");
                        break;
                }
            }
        }
        static void CountVotes()
        {
            if (IsAlreadyCounted) return;

            foreach (var vote in Voters.voters)
            {
                if (vote.VoterChoice != null)
                {
                    if (Candidates.candidates.ContainsKey(vote.VoterChoice))
                    {
                        Candidates.candidates[vote.VoterChoice]++;
                    }
                    else
                    {
                        WrongVotes++;
                    }
                }
            }
            IsAlreadyCounted = true;
        }

        internal static void Vote()
        {
            Console.WriteLine("Enter Your Name");
            string UserName = Console.ReadLine();

            if (CheckVoterValidity(UserName: UserName))
            {
                int VoterIndex = Voters.voters.FindIndex(p => p.Name == UserName);

                if (Voters.voters[VoterIndex].VoterChoice != null)
                {
                    Console.WriteLine("You have already voted. It is not allowed to vote twice");
                }

                else
                {
                    Console.WriteLine("Whom you vote? Bush, Clinton, Trump.");
                    string UserVote = CallOfUserInput();
                    Voters.voters[VoterIndex].VoterChoice = UserVote;
                    if (UserVote != "Buss" && UserVote != "Clinton" && UserVote != "Trump")
                    {
                        Console.WriteLine("You wasted your vote. You cannot vote anymore");
                    }
                    else
                    {
                        Console.WriteLine("Vote successfull");
                    }
                }
            }

            else
            {
                Console.WriteLine("Your name is not listed. You cannot vote");
            }
        }

        private static bool CheckVoterValidity(string UserName)
        {
            foreach (var voter in Voters.voters)
            {
                if (UserName == voter.Name)
                {
                    return true;
                }
            }
            return false;
        }


        internal static void PrintVotingDetails()
        {
            int TotalVoted = 0;
            foreach (Voter voter in Voters.voters)
            {
                if (voter.VoterChoice != null)
                {
                    TotalVoted++;
                }
            }
            Console.WriteLine($"Voted {TotalVoted} people out of {Voters.voters.Count}");
        }

        internal static void AddVoter()
        { 
            Console.WriteLine("Type your name");
            string Name = CallOfUserInput();
            Console.WriteLine("Type your gender: M for male, F for Female");
            string UserInput = CallOfUserInput().ToUpper();

            while (UserInput != "m" & UserInput != "M" & UserInput != "f" & UserInput != "F")
            {
                Console.WriteLine("Wrong answer. Try again");
                UserInput = CallOfUserInput().ToUpper();
            }
            if (UserInput == "M")
            {
                UserInput = "Male";
            }
            else if (UserInput == "F")
            {
                UserInput = "Female";
            }
            Gender Gender = (Gender)Enum.Parse(typeof(Gender), UserInput);

            Console.WriteLine("Type your age");
            int Age = 0;
            bool Parced = false;
            while (Parced == false)
            {
                if (int.TryParse(CallOfUserInput(), out Age))
                {
                    if (Age < 18)
                    {
                        Console.WriteLine("Age must be 18 or greater.");
                    } 
                    else
                    {
                        Parced = true;
                    }
                }
                else
                {
                    Console.WriteLine("Age must be a number.");
                }
            }
            Voters.voters.Add(new Voter(Name, Gender, Age, null));
            Console.WriteLine("Voter added successfully");

            Console.WriteLine("Whom you vote? Bush, Clinton, Trump.");
            string UserVote = CallOfUserInput();
            int VoterIndex = Voters.voters.FindIndex(p => p.Name == Name);
            Voters.voters[VoterIndex].VoterChoice = UserVote;
            if (UserVote != "Buss" && UserVote != "Clinton" && UserVote != "Trump")
            {
                Console.WriteLine("You wasted your vote. You cannot vote anymore");
            }
            else
            {
                Console.WriteLine("Vote successfull");
            }
        }

        internal static void PrintCandidateVotes()
        {
            CountVotes();

            foreach (var candidate in Candidates.candidates)
            {
                Console.WriteLine($"{candidate.Key} : {candidate.Value} votes");
            }
            Console.WriteLine($"Wrong votes : {WrongVotes}");
        }
    }
}
