using Newtonsoft.Json;
using Scheduler.Models;
using System;
using System.ComponentModel;
using System.IO;

namespace Scheduler.Services
{
    class FileIOService
    {
        private readonly string PATH = $"{Environment.CurrentDirectory}\\taskDateList.json";

        public BindingList<TaskModel> loadData()
        {
            var fileExists = File.Exists(PATH);

            if (!fileExists)
                return Creat();

            using (var reader = File.OpenText(PATH))
            {
                var fileText = reader.ReadToEnd();

                var taskDataList = JsonConvert.DeserializeObject<BindingList<TaskModel>>(fileText);
                if (taskDataList != null)
                    return taskDataList;
            }

            return Creat();
        }

        public void SaveData(object taskDataList)
        {
            using (StreamWriter writer = File.CreateText(PATH))
            {
                string output = JsonConvert.SerializeObject(taskDataList);
                writer.Write(output);
            }
        }

        private BindingList<TaskModel> Creat()
        {
            File.CreateText(PATH).Dispose();
            return new BindingList<TaskModel>();
        }
    }
}
