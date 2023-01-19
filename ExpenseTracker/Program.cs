using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
namespace ExpenseTracker
{
    public class Option
    {
        public String Name { get; set; }
        public Action OnSelect { get; set; }

        public Option(String name, Action onSelect)
        {
            this.Name = name;
            this.OnSelect = onSelect;
        }

    }
    public class Expense
    {
        public Double Monto { get; set; }
        public string Tipo { get; set; }
        public bool EstaPago { get; set; }

        public Expense(double amount, string type, bool isPaid)
        {
            Monto = amount;
            Tipo = type;
            EstaPago = isPaid;
        }
    }
    public class Expenses
    {

        public List<Expense> _Expenses = new List<Expense>();
        public List<Option> _Options = new List<Option>();

        public Expense SelectedExpense { get; set; }
        public Option SelectedOption { get; set; }

        public Expenses()
        {
            if (_Expenses.Count > 0)
            {
                this.SelectedExpense = _Expenses[0];
            }
            if (_Options.Count > 0)
            {
                this.SelectedOption = _Options[0];
            }
        }

        public void AddExpense()
        {
            Console.Clear();
            Console.WriteLine("\tINFO: Un Gasto debe tener tipo, monto en USD. Está marcado como impago como predeterminado.");
            Console.Write("\t Tipo:  ");
            var ExpenseType = Console.ReadLine();
            Console.Write("\t Monto: $");
            var _Amount = Convert.ToDouble(Console.ReadLine());
            const bool isPaid = false;

            if (ExpenseType != null)
            {
                Expense ExpenseToAdd = new Expense(_Amount, ExpenseType, isPaid);
                Console.WriteLine($"\t {ExpenseType} Gasto añadido. Validando y guardando en la Lista de Gastos");
                this._Expenses.Add(ExpenseToAdd);
            }
        }

        public void UpdateExpense(Expense expense)
        {
            Console.Clear();
            bool setPaid = !expense.EstaPago;
            Console.WriteLine($"\t#Nota: Setting ${expense.Tipo} gasto de USD ${expense.Monto} a {setPaid.ToString()}.#");
            if (this._Expenses.Contains(expense))
            {
                var ExpenseIndex = this._Expenses.IndexOf(expense);
                this._Expenses[ExpenseIndex].EstaPago = setPaid;
                Console.WriteLine("Gasto actualizado");
                Console.Clear();
            }
        }

        public void RemoveExpense(Expense expense)
        {
            Console.Clear();
            if (this._Expenses.Contains(expense))
            {
                var expenseIndex = this._Expenses.IndexOf(expense);
                this._Expenses.RemoveAt(expenseIndex);
                Console.WriteLine($"\t#INFO: Eliminando {expense.Tipo} gasto de USD ${expense.Monto}.#");
            }
        }

        public void AddOptionsToList(Option option)
        {
            if (option == null)
            {
                Console.WriteLine("Error: Valor no válido proporcionado como argumento. Esperando una opción como argumento");
                return;
            }
            this._Options.Add(option);
        }

    }

    public class Menu
    {
        ConsoleKeyInfo KeyInfo;
        public int ActiveOptionIndex;
        public int ActiveExpenseIndex;

        public Menu(int activeOptionIndex = 0, int activeExpenseIndex = 0)
        {
            this.ActiveOptionIndex = activeOptionIndex;
            this.ActiveExpenseIndex = activeExpenseIndex;
        }

        public void DisplayOptionMenu(List<Option> options, int selectedOptionIndex)
        {
            int index = selectedOptionIndex;
            WriteOptionsMenu(options, options[index]);
            do
            {
                KeyInfo = Console.ReadKey();
                if (KeyInfo.Key == ConsoleKey.DownArrow)
                {
                    if (index + 1 < options.Count)
                    {
                        ++index;
                        WriteOptionsMenu(options, options[index]);
                    }
                }
                if (KeyInfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteOptionsMenu(options, options[index]);
                    }
                }
                if (KeyInfo.Key == ConsoleKey.Enter)
                {
                    options[index].OnSelect.Invoke();
                    index = 0;
                }
            }
            while (KeyInfo.Key != ConsoleKey.Escape);
            Console.ReadKey();
        }
        static void WriteOptionsMenu(List<Option> options, Option selectedOption)
        {
            Console.Clear();

            Console.WriteLine("\n________________________________________________________________________________\n");
            Console.WriteLine("Expense Tracker Application");
            Console.WriteLine("\n________________________________________________________________________________\n");
            foreach (Option option in options)
            {
                if (option == selectedOption)
                {
                    Console.Write("> ");
                }
                else
                {
                    Console.Write("  ");
                }

                Console.WriteLine(option.Name);
            }
        }
        public void ListAllExpenses(List<ExpenseOption> expenses)
        {
            int index = 0;
            WriteExpensesAsTable(expenses, expenses[index].Expense);
            do
            {
                KeyInfo = Console.ReadKey();
                if (KeyInfo.Key == ConsoleKey.DownArrow)
                {
                    if (index + 1 < expenses.Count)
                    {
                        ++index;
                        WriteExpensesAsTable(expenses, expenses[index].Expense);
                    }
                }
                if (KeyInfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteExpensesAsTable(expenses, expenses[index].Expense);
                    }
                }
                if (KeyInfo.Key == ConsoleKey.Enter)
                {
                    expenses[index].OnSelect.Invoke();
                }
            }
            while (KeyInfo.Key != ConsoleKey.Escape);
            Console.ReadKey();

        }
        static void WriteExpensesAsTable(List<ExpenseOption> options, Expense selectedExpense)
        {
            Console.Clear();
            if (options.Count > 0)
            {
                Console.WriteLine("\n________________________________________________________________________________\n");
                Console.WriteLine("\t EXPENSES \t");
                Console.WriteLine("\t###HINT: `O` para eliminar un gasto, `X` para marcar un gasto como pagado ");
                Console.WriteLine("\n________________________________________________________________________________\n");
                Console.WriteLine("\t\t Accion \t\tTipo \t\tMonto \t\tEstaPago");
                foreach (ExpenseOption iterator in options)
                {
                    var expense = iterator.Expense;
                    if (selectedExpense == expense)
                    {
                        Console.Write($"\t\t> {(expense.EstaPago ? "X" : "O")}");
                    }
                    else
                    {
                        Console.Write("\t\t  ");
                    }
                    Console.WriteLine($"\t\t\t{expense.Tipo} \t\tUSD${expense.Monto} \t{expense.EstaPago.ToString()}");
                }
            }
            else
            {
                Console.WriteLine("###Info: No hay gastos para mostrar. Añade un gasto e inténtalo de nuevo");
            }

        }
    }
    public class ExpenseOption
    {
        public Expense Expense { get; set; }
        public Action OnSelect { get; set; }

        public ExpenseOption(Expense expense, Action onSelect)
        {
            Expense = expense;
            OnSelect = onSelect;
        }
    }
    class program
    {

        public static void Main()
        {
            List<ExpenseOption> Options = new List<ExpenseOption>();
            var expenses = new Expenses();
            var expenseMenu = new Menu();
            var AddExpense = new Option("Añadir Expense", () => {
                expenses.AddExpense();
                if (expenses._Expenses.Count > 0)
                {
                    foreach (Expense _expense in expenses._Expenses)
                    {
                        List<Expense> _list = new List<Expense>();
                        foreach (ExpenseOption option in Options)
                        {
                            _list.Add(option.Expense);
                        }
                        if (!_list.Contains(_expense))
                        {
                            {
                                Options.Add(new ExpenseOption(_expense, () => {
                                    if (_expense.EstaPago)
                                    {
                                        expenses.RemoveExpense(_expense);
                                        Options.Remove(Options.Single(Single => Single.Expense == _expense));
                                        Thread.Sleep(1000);
                                        expenseMenu.DisplayOptionMenu(expenses._Options, 0);
                                    }
                                    else
                                    {
                                        expenses.UpdateExpense(_expense);
                                        Thread.Sleep(1000);
                                        expenseMenu.DisplayOptionMenu(expenses._Options, 0);
                                    }
                                }));

                            }

                        }
                    }
                }
                Thread.Sleep(1000);
                expenseMenu.DisplayOptionMenu(expenses._Options, 0);
            });
            var ListExpense = new Option("Lista de Expenses", () => {
                if (Options.Count < 1)
                {
                    Console.WriteLine("#Error: Sin gastos para mostrar. Agrega uno e inténtalo de nuevo.");
                    Thread.Sleep(1000);
                    expenseMenu.DisplayOptionMenu(expenses._Options, 0);
                }
                expenseMenu.ListAllExpenses(Options);
            });

            expenses.AddOptionsToList(AddExpense);
            expenses.AddOptionsToList(ListExpense);

            expenseMenu.DisplayOptionMenu(expenses._Options, 0);
        }
    }
}
