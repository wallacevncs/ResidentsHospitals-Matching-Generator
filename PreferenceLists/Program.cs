using Newtonsoft.Json;

namespace Preference_Lists
{
    class Program
    {
        static void Main(string[] args)
        {

            string residentsFilePath  = Environment.GetEnvironmentVariable("RESIDENTS_FILEPATH");
            string hospitalsPath      = Environment.GetEnvironmentVariable("HOSPITALS_PREFERENCES_FILEPATH");
            string residentsPath      = Environment.GetEnvironmentVariable("RESIDENTS_PREFERENCES_FILEPATH");
            string totalPositionsStr  = Environment.GetEnvironmentVariable("TOTAL_POSITIONS");
            if (String.IsNullOrEmpty(residentsFilePath) || String.IsNullOrEmpty(hospitalsPath) || String.IsNullOrEmpty(residentsPath) || String.IsNullOrEmpty(totalPositionsStr))
            {
                Console.WriteLine("RESIDENTS_FILEPATH, HOSPITALS_PREFERENCES_FILEPATH, RESIDENTS_PREFERENCES_FILEPATH and TOTAL_POSITIONS cannot be empty.");
                return;
            }

            if (!int.TryParse(totalPositionsStr, out int totalPositions))
            {
                Console.WriteLine("TOTAL_POSITIONS is not a valid integer.");
                return;
            }

            Random random      = new Random();
            int currentSize    = 0;
            int index          = 1;
            int weight         = 0;

            List<Hospital> hospitalsPreferences                = new List<Hospital>();
            Dictionary<string, Resident> residentsPreference   = new Dictionary<string, Resident>();
            Dictionary<string, int> residents                  = GetResidents(residentsFilePath, ref residentsPreference);
            if (residents.Count == 0)
            {
                Console.WriteLine("File with residents names cannot be empty.");
                return;
            }

            do
            {
                int capacity             = random.Next(5, 61);
                if (currentSize + capacity <= totalPositions)
                {
                    Hospital hospital    = new Hospital();
                    hospital.Program     = $"Hospital_{index}";
                    hospital.Capacity    = capacity;
                    hospital.Preferences = ChooseResidents(capacity, ref residents, random, ref weight, hospital.Program, ref residentsPreference);
                    hospitalsPreferences.Add(hospital);
                    currentSize         += capacity;
                    index++;
                }
                else
                {
                    int diff = totalPositions - currentSize;
                    if (diff >= 5 && diff <= 60)
                    {
                        Hospital hospital    = new Hospital();
                        hospital.Program     = $"Hospital_{index}";
                        hospital.Capacity    = diff;
                        hospital.Preferences = ChooseResidents(diff, ref residents, random, ref weight, hospital.Program, ref residentsPreference);
                        hospitalsPreferences.Add(hospital);
                        currentSize         += diff;
                    }
                }

            } while (currentSize < totalPositions);

            HospitalPreferences preferences = new HospitalPreferences
            {
                Hospitals                   = hospitalsPreferences,
                Total                       = currentSize
            };

            ResidentPreferences residentPreferences = new ResidentPreferences() { Preferences = new List<Resident>()};
            foreach (KeyValuePair<string, Resident> resident in residentsPreference)
            {
                resident.Value.Preferences = resident.Value.Preferences.OrderBy(index => random.Next()).ToList();
                residentPreferences.Preferences.Add(resident.Value);
            }

            string residentsJson = JsonConvert.SerializeObject(residentPreferences, Formatting.Indented);
            string hospitalsJson = JsonConvert.SerializeObject(preferences, Formatting.Indented);

            File.WriteAllText(hospitalsPath, hospitalsJson);
            File.WriteAllText(residentsPath, residentsJson);
        }

        private static List<string> ChooseResidents(int capacity, ref Dictionary<string, int> residents, Random random, ref int weight, string program, ref Dictionary<string, Resident> residentsPreference)
        {
            //int max     = capacity + (capacity / 2);
            int max     = 2 * capacity;
            int size    = random.Next(capacity, max);
            bool retry  = false;

            Dictionary<string, int> filteredResidents = new Dictionary<string, int>();
            bool hasZeroValue = residents.ContainsValue(0);
            if (hasZeroValue)
            {
                do
                {

                    foreach (var kvp in residents)
                    {
                        if (kvp.Value == weight)
                            filteredResidents.Add(kvp.Key, kvp.Value);

                        if (retry && filteredResidents.Count == size)
                            break;
                    }

                    retry = false;
                    if (filteredResidents.Count < size)
                    {
                        weight++;
                        retry = true;
                    }

                } while (retry);
            }
            else
            {
                filteredResidents = residents;
            }

            List<string> shuffledKeys   = filteredResidents.Keys.OrderBy(key => random.Next()).Take(size).ToList();
            foreach (string key in shuffledKeys)
            {
                residents[key]++;
                residentsPreference[key].Preferences.Add(program);
            }

            return shuffledKeys;
        }

        private static Dictionary<string, int> GetResidents(string residentsFilePath, ref Dictionary<string, Resident> residentsPreference)
        {
            Dictionary<string, int> residents  = new Dictionary<string, int>();
            string[] lines                     = File.ReadAllLines(residentsFilePath);

            foreach (string resident in lines)
            {
                residents[resident]            = 0;
                residentsPreference[resident]  = new Resident() { Name = resident, Preferences = new List<string>()};
            }

            return residents;
        }

    }

    public class Resident
    {
        public string Name                { get; set; }
        public List<string> Preferences   { get; set; }
    }

    public class ResidentPreferences
    {
        public List<Resident> Preferences { get; set; }
    }

    public class HospitalPreferences
    {
        public List<Hospital> Hospitals   { get; set; }
        public int Total                  { get; set; }
    }

    public class Hospital 
    { 
        public string Program             { get; set; }
        public int Capacity               { get; set; }
        public List<string> Preferences   { get; set; }
    }

}
