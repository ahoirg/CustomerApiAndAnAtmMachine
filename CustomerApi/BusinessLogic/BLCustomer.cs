using CustomerApi.Interfaces;
using CustomerApi.Models;

namespace CustomerApi.BusinessLogic
{
    public class BLCustomer : IBLCustomer
    {
        List<MCustomer> datasetCustomer = new List<MCustomer>();

        private string filePath = Path.GetFullPath(Directory.GetCurrentDirectory() + @"\data.txt");
        //It is used to queue the requests when more than one add request is received.
        private object lockObj = new Object();

        public BLCustomer()
        {
            //if there is a data file, it gets the data from the file.
            //Otherwise, it creates a file to hold the data.

            if (!File.Exists(filePath))
                using (FileStream fs = File.Create(filePath)) { }

            ReadDataFileAndSetDataset();
        }

        public List<MMessage> AddCustomer(List<MCustomer> customers)
        {
            List<MMessage> messages = new List<MMessage>();
            //each request can contain more than one customer model.
            //We add the appropriate ones to the list.
            //Those that contain incorrect data return with meaningful error messages.
            List<MCustomer> _customers = new List<MCustomer>();
            
            //When more than one request arrives.
            //It does not allow more than one thread to access the dataset
            //before the insertion process is finished.
            lock (lockObj)
            {
                foreach (var customer in customers)
                {
                    var message = IsValidModel(customer);
                    if (!message.IsSucceed)
                    {
                        messages.Add(message);
                        continue;
                    }
                    _customers.Add(customer);
                }

                InsertCustomers(_customers);
            }
            if (messages.Count == 0)
                messages.Add(new MMessage(isSucceed: true));

            return messages;
        }

        //It returns the data currently in the dataset. It does not wait for post operations.
        public List<MCustomer> GetAllCustomer()
        {
            return datasetCustomer;
        }

        public MMessage IsValidModel(MCustomer customer)
        {
            var toReturn = new MMessage(true, new List<string>());
            if (customer.Age < 18)
                toReturn.Messages.Add("The customer must be at least 18 years old.");

            if (datasetCustomer.Count() > 0 && datasetCustomer.Any(x => x.Id == customer.Id))
                toReturn.Messages.Add("The customer ID you entered has been used before.");

            if (toReturn.Messages.Any())
            {
                toReturn.IsSucceed = false;
                toReturn.Customer = customer;
            }

            return toReturn;
        }

        private void InsertCustomers(List<MCustomer> customers)
        {
            foreach (var customer in customers)
                InsertCustomers(customer);

            UpdateDataFile();
        }

        private void InsertCustomers(MCustomer customer)
        {
            lock (lockObj)
            {
                if (datasetCustomer.Any())
                {
                    //First, it checks the equality according to the lastname. Then according to the firstname.
                    var sameLastNames = datasetCustomer.Where(x => x.LastName.CompareTo(customer.LastName) == 0);
                    if (sameLastNames.Any())
                    {
                        //If it finds equality according to first name,
                        //insert operation takes place from the index of the model that is equal to firstname.

                        var sameFirstNameIds = sameLastNames.Where(x => x.FirstName.CompareTo(customer.LastName) == 0).Select(x => x.Id);
                        if (sameFirstNameIds.Any())
                            datasetCustomer.Insert(datasetCustomer.FindIndex(x => x.Id == sameFirstNameIds.First()), customer);
                        else
                        {
                            //otherwise it uses an index of a value from LastName.
                            datasetCustomer.Insert(datasetCustomer.FindIndex(x => x.Id == sameLastNames.First().Id), customer);
                        }
                    }
                    else
                    {
                        //if the customer's lastname is not in the dataset.
                        //Adds customer information to the dataset in alphabetical order of last name.
                        var possibleIndex = datasetCustomer.FirstOrDefault(x => x.LastName.CompareTo(customer.LastName) > 0);
                        if (possibleIndex != null)
                            datasetCustomer.Insert(datasetCustomer.FindIndex(x => x.Id == possibleIndex.Id), customer);
                        else
                            datasetCustomer.Add(customer);
                    }
                }
                else
                {
                    //If there is no data in the data set, there is no need for control steps.
                    datasetCustomer.Add(customer);
                }
            }
        }

        //Updates data.txt
        private void UpdateDataFile()
        {
            List<string> data = new List<string>();
            foreach (var customer in datasetCustomer)
            {
                string text = customer.Id.ToString() + ","
                + customer.Age.ToString() + ","
                + customer.FirstName.ToString() + ","
                + customer.LastName.ToString();

                data.Add(text);

            }
            File.WriteAllText(filePath, string.Join("\n", data.Select(i => i.ToString())));
        }

        //Reads from data.txt
        private void ReadDataFileAndSetDataset()
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                var file = sr.ReadLine();
                var lines = File.ReadLines(filePath);
                foreach (var line in lines)
                {
                    var splitedData = line.Split(",");
                    var id = int.Parse(splitedData[0]);
                    var age = int.Parse(splitedData[1]);

                    MCustomer newCustomer = new MCustomer(id: id, age: age, firstName: (string)splitedData[2], lastName: splitedData[3]);
                    InsertCustomers(newCustomer);
                }
            }
        }
    }
}
