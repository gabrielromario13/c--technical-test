
internal class TechnicalTest
{
    private const string CSV_FILE_PATH = @"C:\Sorted_Academy_Candidates.csv";

    private static readonly IEnumerable<string> AcademyCandidates = File.ReadLines(@"C:\Academy_Candidates.txt");

    internal static void Main()
    {
        var academyCandidates = GetAcademyCandidates();
        var candidatesPerVacancy = CountCandidatesPerVacancy(academyCandidates);
        var candidatesAgePerVacancy = GetAgesPerVacancy(academyCandidates);

        //Task 1
        PrintPercentageOfCandidatesPerVacancy(candidatesPerVacancy, academyCandidates.Count);

        //Task 2
        PrintAverageAgeByVacancy("QA", candidatesAgePerVacancy);

        //Task 3
        PrintOldestCandidateAgeByVacancy("Mobile", candidatesAgePerVacancy);

        //Task 4
        PrintYoungestCandidateAgeByVacancy("Web", candidatesAgePerVacancy);

        //Task 5
        PrintCombinedAges("QA", candidatesAgePerVacancy);

        //Task 6
        PrintDistinctStatesCount(academyCandidates);

        //Task 7
        GenerateCsvFile();

        //Task 8
        PrintQualityAssuranceInstructorsFullName(academyCandidates);

        //Task 9
        PrintMobileInstructorsFullName(academyCandidates);
    }

    private static List<AcademyCandidate> GetAcademyCandidates()
    {
        var candidates = new List<AcademyCandidate>();
        var academyCandidates = AcademyCandidates.Skip(1);

        foreach (var academyCandidate in academyCandidates)
        {
            var candidate = academyCandidate.Split(';');
            candidates.Add(new AcademyCandidate
            (
                candidate[0],
                int.Parse(candidate[1].Remove(2, 5)),
                candidate[2],
                candidate[3]
            ));
        }

        return candidates;
    }

    private static Dictionary<string, int> CountCandidatesPerVacancy(
        List<AcademyCandidate> candidates)
    {
        var candidatesPerVacancy = new Dictionary<string, int>();

        foreach (var candidate in candidates)
        {
            if (candidatesPerVacancy.ContainsKey(candidate.Vacancy))
            {
                candidatesPerVacancy[candidate.Vacancy]++;
            }
            else
            {
                candidatesPerVacancy[candidate.Vacancy] = 1;
            }
        }

        return candidatesPerVacancy;
    }

    private static Dictionary<string, List<int>> GetAgesPerVacancy(
        List<AcademyCandidate> candidates)
    {
        var candidatesAgePerVacancy = new Dictionary<string, List<int>>();

        foreach (var candidate in candidates)
        {
            if (!candidatesAgePerVacancy.ContainsKey(candidate.Vacancy))
            {
                candidatesAgePerVacancy[candidate.Vacancy] = [];
            }
            candidatesAgePerVacancy[candidate.Vacancy].Add(candidate.Age);
        }

        return candidatesAgePerVacancy;
    }

    private static void PrintPercentageOfCandidatesPerVacancy(
        Dictionary<string, int> candidatesPerVacancy,
        int totalCandidates)
    {
        Console.WriteLine("1. Proporção de candidatos por vaga:\n");
        foreach (var vacancy in candidatesPerVacancy)
        {
            double percentage = (vacancy.Value / (double)totalCandidates) * 100;
            Console.WriteLine($"   {vacancy.Key}: {percentage:F2}%");
        }
        Console.WriteLine("");
    }

    private static void PrintAverageAgeByVacancy(
        string vacancy,
        Dictionary<string, List<int>> candidatesAgePerVacancy)
    {
        if (candidatesAgePerVacancy.TryGetValue(vacancy, out var ages))
        {
            var averageAge = ages.Average();
            Console.WriteLine($"2. Idade média dos candidatos de {vacancy}:\n\n   " +
                              $"R: {(int)averageAge} anos\n");
        }
    }

    private static void PrintOldestCandidateAgeByVacancy(
        string vacancy, Dictionary<string,
        List<int>> candidatesAgePerVacancy)
    {
        if (candidatesAgePerVacancy.TryGetValue(vacancy, out var ages))
        {
            //int oldest = ages.Max();
            int oldest = int.MinValue;

            foreach (var age in ages)
                if (age > oldest) oldest = age;

            Console.WriteLine($"3. Idade do candidato mais velho de {vacancy}:\n\n   " +
                              $"R: {oldest} anos\n");
        }
    }

    private static void PrintYoungestCandidateAgeByVacancy(
        string vacancy, Dictionary<string,
        List<int>> candidatesAgePerVacancy)
    {
        if (candidatesAgePerVacancy.TryGetValue(vacancy, out var ages))
        {
            //double youngest = ages.Min();
            int youngest = int.MaxValue;

            foreach (var age in ages)
            {
                if (age < youngest)
                    youngest = age;
            }

            Console.WriteLine($"4. Idade do candidato mais novo de {vacancy}:\n\n   " +
                              $"R: {youngest} anos\n");
        }
    }

    private static void PrintCombinedAges(
        string vacancy, Dictionary<string,
        List<int>> candidatesAgePerVacancy)
    {
        if (candidatesAgePerVacancy.TryGetValue(vacancy, out var ages))
        {
            //double combinedAges = ages.Sum();
            double combinedAges = 0;

            foreach (var age in ages)
                combinedAges += age;

            Console.WriteLine($"5. Soma das idades dos candidatos de {vacancy}:\n\n   " +
                              $"R: {combinedAges}\n");
        }
    }

    private static void PrintDistinctStatesCount(List<AcademyCandidate> candidates)
    {
        var states = candidates.Select(x => x.State);

        var distinctStates = states.Distinct().ToList();

        Console.WriteLine($"6. Número de estados distintos presentes entre os candidatos:\n\n   " +
                          $"R: {distinctStates.Count}\n");
    }

    private static void GenerateCsvFile()
    {
        var academyCandidates = AcademyCandidates;

        var header = academyCandidates.First();

        var data = academyCandidates.Skip(1).OrderBy(x => x);

        var finalData = new List<string> { header };
        finalData.AddRange(data);

        File.WriteAllLines(CSV_FILE_PATH, finalData);

        var csvFileExists = File.Exists(CSV_FILE_PATH);

        Console.WriteLine(
            $"7. Confirmação de que o arquivo `Sorted_Academy_Candidates.csv` foi criado:\n\n   " +
            $"R: {csvFileExists} (File.Exists(CSV_FILE_PATH))\n");
    }

    private static void PrintQualityAssuranceInstructorsFullName(List<AcademyCandidate> candidates)
    {
        var filteredCandidates = candidates.Where(
            x => x.Vacancy == "QA" &&
            x.State == "SC" &&
            x.Age >= 18 &&
            x.Age <= 30
        ).ToList();

        foreach (var candidate in filteredCandidates)
        {
            var root = (int)Math.Sqrt(candidate.Age);
            var isPerfectSquareNumber = root * root == candidate.Age;

            var isPalindrome = IsPalindrome(candidate);

            if (isPerfectSquareNumber && isPalindrome)
                Console.WriteLine($"8. O nome do instrutor de QA descoberto:\n\n   " +
                                  $"R: {candidate.FullName}\n");
        }
    }

    private static void PrintMobileInstructorsFullName(List<AcademyCandidate> candidates)
    {
        var filteredCandidates = candidates.Where(
            x => x.Vacancy == "Mobile" &&
            x.State == "PI" &&
            x.Age >= 30 &&
            x.Age <= 40 &&
            x.Age % 2 == 0
        ).ToList();

        foreach (var candidate in filteredCandidates)
        {
            var lastName = candidate.FullName.Split(" ")[1];

            if (lastName.First() == 'C')
                Console.WriteLine($"9. O nome do instrutor de Mobile descoberto:\n\n   " +
                                  $"R: {candidate.FullName}\n");
        }
    }

    private static bool IsPalindrome(AcademyCandidate instructor)
    {
        var firstName = instructor.FullName.Split(" ").First();
        firstName = firstName.ToLower();
        string first = firstName.Substring(0, firstName.Length / 2);
        char[] arr = firstName.ToCharArray();

        Array.Reverse(arr);

        string temp = new String(arr);
        string second = temp.Substring(0, temp.Length / 2);

        return first.Equals(second);
    }
}

internal class AcademyCandidate
{
    public string FullName { get; protected set; } = string.Empty;
    public int Age { get; protected set; }
    public string Vacancy { get; protected set; } = string.Empty;
    public string State { get; protected set; } = string.Empty;

    public AcademyCandidate(string fullName, int age, string vacancy, string state)
    {
        FullName = fullName;
        Age = age;
        Vacancy = vacancy;
        State = state;
    }
}