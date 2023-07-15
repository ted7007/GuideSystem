using System;
using System.Collections.Generic;
using System.IO;
using GuideSystemApp.Student.Hash;
using GuideSystemApp.Student.List;
using GuideSystemApp.Student.RB;

namespace GuideSystemApp.Student
{
    public class StudentRepository
    {
        public List<Student> StudentArray;
        public HashTable HashTable;
        private RB_Tree StudentFIO;
        private RB_Tree Group;
        private RB_Tree AdmissionDate;

        public StudentRepository(int count)
        {
            this.StudentArray = new List<Student>();
            StudentFIO = new RB_Tree();
            Group = new RB_Tree();
            AdmissionDate = new RB_Tree();
            HashTable = new HashTable(count);
        }

        public void ReadFromFile(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                int count = int.Parse(reader.ReadLine()); // Преобразование строки в число

                // Записываем данные в массив
                for (int i = 0; i < count; i++)
                {
                    string[] studentstr = reader.ReadLine().Split('/');
                    Student student = new Student(studentstr[0], studentstr[1], studentstr[2], studentstr[3]);
                    StudentArray.Add(student);
                }
            }

            CreateIndexes();
        }

        private void CreateIndexes()
        {
            for (int i = 0; i < StudentArray.Count; i++)
            {
                StudentFIO.Insert(StudentArray[i].FIO, i);
                Group.Insert(StudentArray[i].Group, i);
                AdmissionDate.Insert(StudentArray[i].AdmissionDate, i);
                HashTable.Add(StudentArray[i].Passport, i);
            }
        }

        public void Add(Student student)
        {
            if (!Validate(student))
            {
                Console.WriteLine("Неверные данные студента. Не удалось добавить студента.");
                return;
            }

            StudentArray.Add(student);
            AddToIndexes(StudentArray.Count - 1);
        }

        private void AddToIndexes(int i)
        {
            StudentFIO.Insert(StudentArray[i].FIO, i);
            Group.Insert(StudentArray[i].Group, i);
            AdmissionDate.Insert(StudentArray[i].AdmissionDate, i);
            HashTable.Add(StudentArray[i].Passport, i);
        }

        public List<Student> GetAll()
        {
            var marks = new List<Student>();
            for (int i = 0; i < StudentArray.Count; i++)
            {
                marks.Add(StudentArray[i]);
                marks[i].Index = i;
            }

            return marks;
        }

        public void WriteToFile(string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(StudentArray.Count);

                foreach (var student in StudentArray)
                {
                    writer.WriteLine($"{student.FIO}/{student.Group}/{student.Passport}/{student.AdmissionDate}");
                }
            }
        }

        public void Delete(Student student)
        {
            int res = Find(student);
            if (res == -1 || res >= StudentArray.Count)
                return;

            var removeItem = StudentArray[res];
            int lastIndex = StudentArray.Count - 1;

            if (res == lastIndex)
            {
                RemoveFromIndexes(res);
                StudentArray.RemoveAt(res);
            }
            else
            {
                StudentArray[res] = StudentArray[lastIndex];
                StudentArray.RemoveAt(lastIndex);

                UpdateIndexesAfterDelete(removeItem, lastIndex, res);
            }
        }

        private void RemoveFromIndexes(int i)
        {
            StudentFIO.Delete(StudentArray[i].FIO, i);
            Group.Delete(StudentArray[i].Group, i);
            AdmissionDate.Delete(StudentArray[i].AdmissionDate, i);
            HashTable.Delete(StudentArray[i].Passport, i);
        }

        private void UpdateIndexesAfterDelete(Student student, int oldIndex, int newIndex)
        {
            StudentFIO.EditValue(student.FIO, oldIndex, newIndex);
            Group.EditValue(student.Group, oldIndex, newIndex);
            AdmissionDate.EditValue(student.AdmissionDate, oldIndex, newIndex);
            HashTable.Edit(student.Passport, newIndex);
        }

        public int Find(Student student)
        {
            int comparisonCount = 0;

            // Поиск по хеш-таблице
            var res = HashTable.Search(student.Passport, out int hashTableComparisons);
            comparisonCount += hashTableComparisons;
            if (res != -1)
                return res;

            // Поиск по деревьям
            res = StudentFIO.Search(student.FIO, out int studentFIOComparisons);
            comparisonCount += studentFIOComparisons;
            if (res != -1)
                return res;

            res = Group.Search(student.Group, out int groupComparisons);
            comparisonCount += groupComparisons;
            if (res != -1)
                return res;

            res = AdmissionDate.Search(student.AdmissionDate, out int admissionDateComparisons);
            comparisonCount += admissionDateComparisons;

            Console.WriteLine($"Количество сравнений при поиске: {comparisonCount}");
            return res;
        }

        public List<TreeNode> GetStudentFIO()
        {
            return StudentFIO.GetNodes();
        }

        public List<TreeNode> GetStudentGroup()
        {
            return Group.GetNodes();
        }

        public List<TreeNode> GetStudentAdmissionDate()
        {
            return AdmissionDate.GetNodes();
        }

        public List<KeyValuePair<string, int>> GetHashTable()
        {
            return HashTable.GetItems();
        }

        private bool Validate(Student student)
        {
            // Валидация данных студента
            if (string.IsNullOrEmpty(student.FIO) || string.IsNullOrEmpty(student.Group) ||
                string.IsNullOrEmpty(student.Passport) || string.IsNullOrEmpty(student.AdmissionDate))
            {
                return false;
            }

            return true;
        }

        public void PrintStudentFIO()
        {
            Console.WriteLine("Дерево по ФИО студентов:");
            var studentFIO = GetStudentFIO();
            foreach (var node in studentFIO)
            {
                var indices = string.Join(", ", node.List.GetAllIndices());
                Console.WriteLine($"{node.Key} (Indices: {node.value}{(string.IsNullOrEmpty(indices) ? "" : ", " + indices)})");
            }
        }

        public void PrintStudentGroup()
        {
            Console.WriteLine("Дерево по группам студентов:");
            var studentGroup = GetStudentGroup();
            foreach (var node in studentGroup)
            {
                var indices = string.Join(", ", node.List.GetAllIndices());
                Console.WriteLine($"{node.Key} (Indices: {node.value}{(string.IsNullOrEmpty(indices) ? "" : ", " + indices)})");
            }
        }

        public void PrintStudentAdmissionDate()
        {
            Console.WriteLine("Дерево по дате поступления студентов:");
            var studentAdmissionDate = GetStudentAdmissionDate();
            foreach (var node in studentAdmissionDate)
            {
                var indices = string.Join(", ", node.List.GetAllIndices());
                Console.WriteLine($"{node.Key} (Indices: {node.value}{(string.IsNullOrEmpty(indices) ? "" : ", " + indices)})");
            }
        }

        public void PrintPassportHashTable()
        {
            Console.WriteLine("Хэш-таблица студентов:");
            var hashTable = GetHashTable();
            foreach (var item in hashTable)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
        }
    }
}
