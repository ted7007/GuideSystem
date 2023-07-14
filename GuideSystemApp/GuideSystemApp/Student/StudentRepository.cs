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

        public StudentRepository()
        {
            this.StudentArray = new List<Student>();
            StudentFIO = new RB_Tree();
            Group = new RB_Tree();
            AdmissionDate = new RB_Tree();
            HashTable = new HashTable(20);
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
            return StudentArray;
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
            if (res == -1)
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

        private int Find(Student student)
        {
            // Поиск по хеш-таблице
            var res = HashTable.Search(student.Passport);
            if (res != -1)
                return res;

            // Поиск по деревьям
            res = StudentFIO.Search(student.FIO);
            if (res != -1)
                return res;

            res = Group.Search(student.Group);
            if (res != -1)
                return res;

            res = AdmissionDate.Search(student.AdmissionDate);
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
    }
}
