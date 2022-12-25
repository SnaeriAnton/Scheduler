using Newtonsoft.Json;
using Scheduler.Models;
using System;
using System.ComponentModel;
using System.IO;

namespace Scheduler.Services
{
    class FileIOService
    {
        private readonly string PATH = $"{Environment.CurrentDirectory}\\todoDateList.json";

        public BindingList<TaskModel> loadData()
        {
            var fileExists = File.Exists(PATH);
            if (!fileExists)
            {
                File.CreateText(PATH).Dispose();
                return new BindingList<TaskModel>();
            }

            using (var reader = File.OpenText(PATH))
            {
                var fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<BindingList<TaskModel>>(fileText);
            }
        }

        public void SaveData(object todoDataList)
        {
            using (StreamWriter writer = File.CreateText(PATH))
            {
                string output = JsonConvert.SerializeObject(todoDataList);
                writer.Write(output);
            }
        }
    }
}
