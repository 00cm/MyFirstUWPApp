using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage.Streams;
using Windows.Storage;
using Newtonsoft.Json;

namespace MyFirstUWPApp.DataProvider
{
    public class StudentDataProvider
    {
        private static readonly string _studentsFileName = "students.json";
        private static readonly StorageFolder _localFolder = ApplicationData.Current.LocalFolder;

        public async Task<IEnumerable<Student>> LoadStudentAsync()
        {
            var storageFile = await _localFolder.TryGetItemAsync(_studentsFileName) as StorageFile;
            List<Student> studentList = null;

            if (storageFile == null)
            {
                studentList = new List<Student>
                {
                    new Student("Thomas", "Smith", "Math", "A", 123),
                    new Student("Julia", "Johnson", "Science", "B", 456),
                    new Student("Anna", "Williams", "History", "C", 789)
                };
            }
            else
            {
                using (var stream = await storageFile.OpenAsync(FileAccessMode.Read))
                {
                    using (var dataReader = new DataReader(stream))
                    {
                        await dataReader.LoadAsync((uint)stream.Size);
                        var json = dataReader.ReadString((uint)stream.Size);
                        studentList = JsonConvert.DeserializeObject<List<Student>>(json);
                    }
                }
            }

            return studentList;
        }

        public async Task SaveStudentAsync(List<Student> student)
        {
            var storageFile = await _localFolder.CreateFileAsync(_studentsFileName, CreationCollisionOption.ReplaceExisting);

            using (var stream = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (var dataWriter = new DataWriter(stream))
                {
                    var json = JsonConvert.SerializeObject(student, Newtonsoft.Json.Formatting.Indented);
                    dataWriter.WriteString(json);
                    await dataWriter.StoreAsync();
                }

            }
        }
    }
}
