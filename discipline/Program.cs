﻿internal class Program
{

    private static void Main(string[] args)
    {
        // Добавление данных в массив
        string path = "input.txt";
        Discipline[] data = Input.read(path);
        HachTable table = CreateTable.create(data);

        table.Print();
    }
}