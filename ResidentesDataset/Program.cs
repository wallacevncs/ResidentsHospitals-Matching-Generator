namespace ResidentesDataset
{
    class Program
    {
        static void Main(string[] args)
        {

            string inputFilepath        = Environment.GetEnvironmentVariable("INPUT_FILEPATH");
            string outputFilepath       = Environment.GetEnvironmentVariable("OUTPUT_FILEPATH");
            string totalApplicantsStr   = Environment.GetEnvironmentVariable("TOTAL_APPLICANTS");
            if (String.IsNullOrEmpty(inputFilepath) || String.IsNullOrEmpty(outputFilepath) || String.IsNullOrEmpty(totalApplicantsStr))
            {
                Console.WriteLine("INPUT_FILEPATH, OUTPUT_FILEPATH and TOTAL_APPLICANTS cannot be empty.");
                return;
            }

            if (!int.TryParse(totalApplicantsStr, out int totalApplicants))
            {
                Console.WriteLine("TOTAL_APPLICANTS is not a valid integer.");
                return;
            }

            // Reads a csv file with names of resident doctors
            List<string> residents  = File.ReadAllLines(inputFilepath).ToList();
            if (residents.Count == 0)
            {
                Console.WriteLine("File with applicant names cannot be empty.");
                return;
            }

            List<string> finalList  = new List<string>();
            Random random           = new Random();
            // Randomly choose names to be applying doctors up to the limit defined in total applications
            while (finalList.Count < totalApplicants && residents.Count > 0)
            {
                int indiceAleatorio = random.Next(residents.Count);
                finalList.Add(residents[indiceAleatorio]);
                residents.RemoveAt(indiceAleatorio);
            }

            File.WriteAllLines(outputFilepath, finalList);
        }

    }
}
