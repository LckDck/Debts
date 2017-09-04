using System;
using System.Collections.Generic;
using debtsTracker.Entities;
using debtsTracker.Utilities;

namespace debtsTracker.ViewModels
{
    public class MainViewModel : BaseVm
    {
        public MainViewModel ()
        {
        }

        public void ShowDetails (Debt debt)
        {
            NavigationService.NavigateTo (Page.HistoryPage, debt);
        }

        public List<Debt> GetItems (bool myDebts)
        {
           // if (myDebts) return new List<Debt> ();
            var result = new List<Debt> () {

                new Debt { Name = "Anna",
                    Transactions = new List<Transaction> {
                        new Transaction { Value = 1100, Date = DateTime.Now, Comment="Какой-то коммент"},
                        new Transaction { Value = -300, Date = DateTime.Now, Comment=""},
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" },
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" }
                    }
                },
                new Debt { Name = "Michael",
                    Transactions = new List<Transaction> {
                        new Transaction { Value = 2000, Date = DateTime.Now},
                        new Transaction { Value = 150, Date = DateTime.Now}
                    }
                },
                new Debt { Name = "Michael",
                    Transactions = new List<Transaction> {
                        new Transaction { Value = 2000, Date = DateTime.Now},
                        new Transaction { Value = 150, Date = DateTime.Now}
                    }
                },
                new Debt { Name = "Michael",
                    Transactions = new List<Transaction> {
                        new Transaction { Value = 2000, Date = DateTime.Now},
                        new Transaction { Value = 150, Date = DateTime.Now}
                    }
                },
                new Debt { Name = "Michael",
                    Transactions = new List<Transaction> {
                        new Transaction { Value = 2000, Date = DateTime.Now},
                        new Transaction { Value = 150, Date = DateTime.Now}
                    }
                },
                new Debt { Name = "Michael",
                    Transactions = new List<Transaction> {
                        new Transaction { Value = 2000, Date = DateTime.Now},
                        new Transaction { Value = 150, Date = DateTime.Now}
                    }
                },
                new Debt { Name = "Michael",
                    Transactions = new List<Transaction> {
                        new Transaction { Value = 2000, Date = DateTime.Now},
                        new Transaction { Value = 150, Date = DateTime.Now}
                    }
                },
                new Debt { Name = "Michael",
                    Transactions = new List<Transaction> {
                        new Transaction { Value = 2000, Date = DateTime.Now},
                        new Transaction { Value = 150, Date = DateTime.Now}
                    }
                },
                new Debt { Name = "Michael",
                    Transactions = new List<Transaction> {
                        new Transaction { Value = 2000, Date = DateTime.Now},
                        new Transaction { Value = 150, Date = DateTime.Now}
                    }
                },
                new Debt { Name = "Michael",
                    Transactions = new List<Transaction> {
                        new Transaction { Value = 2000, Date = DateTime.Now},
                        new Transaction { Value = 150, Date = DateTime.Now}
                    }
                },
                new Debt { Name = "Michael",
                    Transactions = new List<Transaction> {
                        new Transaction { Value = 2000, Date = DateTime.Now},
                        new Transaction { Value = 150, Date = DateTime.Now}
                    }
                },
                new Debt { Name = "Michael",
                    Transactions = new List<Transaction> {
                        new Transaction { Value = 2000, Date = DateTime.Now},
                        new Transaction { Value = 150, Date = DateTime.Now}
                    }
                }
            };

            return result;
        }

        internal void AddPage ()
        {
            NavigationService.NavigateTo (Page.AddPage);
        }
    }
}
