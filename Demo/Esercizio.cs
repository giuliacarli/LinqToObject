using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    public class Esercizio
    {
        // creazione liste
        public static List<Product> CreateProductList()
        {
            var lista = new List<Product>
            {
                new Product { Id = 1, Name = "Telefono", UnitPrice = 300.99 },
                new Product { Id = 2, Name = "Computer", UnitPrice = 800 },
                new Product { Id = 2, Name = "Tablet", UnitPrice = 550.99 }
            };
            return lista;
        }

        public static List<Order> CreateOrderList() {
            var lista = new List<Order>();

            var order = new Order
            {
                Id = 1,
                ProductId = 1,
                Quantity = 4 
            };

            lista.Add(order);

            var order1 = new Order
            {
                Id = 2,
                ProductId = 2,
                Quantity = 1
            };

            lista.Add(order1);

            var order2 = new Order
            {
                Id = 3,
                ProductId = 1,
                Quantity = 1
            };

            lista.Add(order2);

            return lista;
        }

        public static void DeferredExecution()
        {
            var productList = CreateProductList();
            var orderList = CreateOrderList();

            //vediamo i risultati
            foreach(var p in productList)
            {
                Console.WriteLine("{0} - {1} - {2}", p.Id, p.Name, p.UnitPrice);
            }

            foreach (var o in orderList)
            {
                Console.WriteLine("{0} - {1} - {2}", o.Id, o.ProductId, o.Quantity);
            }

            //Creazione query
            var list = productList
                .Where(product => product.UnitPrice >= 400)
                .Select(p => new { Nome = p.Name, Prezzo = p.UnitPrice });

            //Aggiungiamo prodotto

            productList.Add(new Product
            {
                Id = 4,
                Name = "Bici",
                UnitPrice = 500.99
            });

            //Risultati
            //  ------> ESECUZIONE DIFFERITA <-------

            Console.WriteLine("Esecuzione differita: ");
            foreach(var p in list)
            {
                Console.WriteLine("{0} - {1}", p.Nome, p.Prezzo);
            }

            // Esecuzione immediata

            var list1 = productList
                .Where(p => p.UnitPrice >= 400)
                .Select(p => new { Nome = p.Name, Prezzo = p.UnitPrice })
                .ToList();

            productList.Add(new Product { Id = 5, Name = "Divano", UnitPrice = 450.99 });

            //Risultati
            Console.WriteLine("Esecuzione immediata: ");

            foreach(var p in list1)
            {
                Console.WriteLine("{0} - {1}", p.Nome, p.Prezzo);
            }

        }

        //Sintassi

        public static void Syntax()
        {
            var productList = CreateProductList();
            var orderList = CreateOrderList();

            //Method Syntax
            var methodList = productList.Where(p => p.UnitPrice >= 600)
                .Select(p => new { Nome = p.Name, Prezzo = p.UnitPrice })
                .ToList();

            //Query
            var queryList =
                (from p in productList
                where p.UnitPrice <= 600
                select new { Nome = p.Name, Prezzo = p.UnitPrice }).ToList();
        }

        public static void Operators()
        {
            var productList = CreateProductList();
            var orderList = CreateOrderList();

            // Scrittura a schermo delle liste
            Console.WriteLine("Lista prodotti: ");

            foreach (var p in productList)
            {
                Console.WriteLine("{0} - {1} - {2}", p.Id, p.Name, p.UnitPrice);
            }

            Console.WriteLine("Lista ordini: ");

            foreach (var o in orderList)
            {
                Console.WriteLine("{0} - {1} - {2}", o.Id, o.ProductId, o.Quantity);
            }


            // Filtro OfType
            var list = new ArrayList();
            list.Add(productList);
            list.Add("Ciao");
            list.Add(123);

            var typeQuery =
                from item in list.OfType<int>()
                select item;
            Console.WriteLine("OfType: ");

            foreach (var item in typeQuery)
            {
                Console.WriteLine(item);
            }

            //Element
            Console.WriteLine("Elementi: ");
            int[] empty = { };
            //var el1 = empty.First(); // mi genera un errore perché non trova niente all'interno di empty
            var el1 = empty.FirstOrDefault(); // mi fa vedere zero perché empty è vuoto e il default di una variabile int vuota è 0
            Console.WriteLine(el1);

            var p1 = productList.ElementAt(0).Name;
            Console.WriteLine(p1);

            //Ordinamento
            Console.WriteLine("Ordinamento: ");

            var orderedList =
                from p in productList
                orderby p.Name ascending, p.UnitPrice descending
                select new { Nome = p.Name, Prezzo = p.UnitPrice };
                                                                            // due modi differenti per fare la stessa cosa (sopra e sotto)
            var orderedList2 = productList
                .OrderBy(p => p.Name)
                .ThenByDescending(p => p.UnitPrice)
                .Select(p => new { Nome = p.Name, Prezzo = p.UnitPrice })
                .Reverse();

            foreach (var p in orderedList)
            {
                Console.WriteLine("{0} - {1}", p.Nome, p.Prezzo);
            }

            foreach (var p in orderedList2)
            {
                Console.WriteLine("{0} - {1}", p.Nome, p.Prezzo);
            }

            //Quantificatori
            var hasProductWithT = productList.Any(p => p.Name.StartsWith("T"));
            var allProductsWithT = productList.All(p => p.Name.StartsWith("T"));
            Console.WriteLine("Ci sono prodotti che iniziano con la t? {0}", hasProductWithT);
            Console.WriteLine("Tutti i prodotti che iniziano con la t? {0}", allProductsWithT);

            //GroupBy
            Console.WriteLine();
            Console.WriteLine("GroupBy: ");

            //Query Syntax
            //Raggruppiamo gli order per ProductId
            var groupByList =
                from o in orderList
                group o by o.ProductId into groupList
                select groupList;

            foreach (var order in groupByList)
            {
                Console.WriteLine(order.Key);
                foreach (var item in order)
                {
                    Console.WriteLine($"\t {item.ProductId} - {item.Quantity}");
                }
            }
                                                      // GROUPBY mi restituisce una lista che contiene oggetti che contengono una chiave (che è quella per cui ho raggruppato) e una lista di oggetti(raggruppati)
            //Method Syntax
            var groupByList2 =
                orderList
                .GroupBy(o => o.ProductId);

            foreach(var order in groupByList)
            {
                Console.WriteLine(order.Key);
                foreach(var item in order)
                {
                    Console.WriteLine("{0} - {1}", item.ProductId, item.Quantity);
                }
            }

            // GroupBy con funzione di aggregazione
            // Raggruppare gli ordini per prodotto e ricavare la somma delle quantità

            var sumQuantityByProduct =                  //Method Syntax
                orderList
                .GroupBy(p => p.ProductId)
                .Select(lista => new
                {
                    Id = lista.Key,
                    Quantities = lista.Sum(p => p.Quantity)
                });

            Console.WriteLine("Groupby con Aggregato: ");
            foreach(var item in sumQuantityByProduct)
            {
                Console.WriteLine("{0} - {1}", item.Id, item.Quantities);
            }

            var sumByProduct2 =                         //Query Syntax
                from o in orderList
                group o by o.ProductId into list3
                select new { Id = list3.Key, Quantities = list3.Sum(x => x.Quantity) };

            foreach (var item in sumByProduct2)
            {
                Console.WriteLine("{0} - {1}", item.Id, item.Quantities);
            }



            //Join
            //Recuperiamo i prodotti che hanno ordini
            //Nome - id ordine - Quantità <---- ciò che vogliamo vedere
            Console.WriteLine("Join: ");

            //Method Syntax

            var joinList = productList
                .Join(orderList,
                p => p.Id,
                o => o.ProductId,
                (p, o) => new { Nome = p.Name, OrderId = o.Id, Quantita = o.Quantity });

            foreach (var p in joinList) 
            {
                Console.WriteLine("{0} - {1} - {2}", p.Nome, p.OrderId, p.Quantita);
            }

            //Query Syntax

            var joinedList2 =
                from p in productList
                join o in orderList
                on p.Id equals o.ProductId
                select new
                {
                    Nome = p.Name,
                    OrderId = o.Id,
                    Quantita = o.Quantity
                };

            foreach (var p in joinedList2)
            {
                Console.WriteLine("{0} - {1} - {2}", p.Nome, p.OrderId, p.Quantita);
            }


            //GroupJoin
            //Recuperare gli ordini per prodotto e somma quantità
            //Nome prodotto - Quantità totale

            var groipJoinList = productList
                .GroupJoin(orderList,
                p => p.Id,
                o => o.ProductId,
                (p, o) => new { Prodotto = p.Name, Quantita = o.Sum(o => o.Quantity) }); //si raggruppa per il primo valore che si mette dopo 'new {'

            foreach (var p in groipJoinList)
            {
                Console.WriteLine("{0} - {1}", p.Prodotto, p.Quantita);
            }

            var groupJoinList2 =
                from p in productList
                join o in orderList
                on p.Id equals o.ProductId
                into gr
                select new
                {
                    Prodotto = p.Name,
                    Quantita = gr.Sum(o => o.Quantity)
                };

            foreach (var item in groupJoinList2)
            {
                Console.WriteLine("{0} - {1}", item.Prodotto, item.Quantita);
            }

            //Lista ProdottoName - Order
            var lista4 =
                from o in orderList
                group o by o.ProductId
                into gr
                select new { ProdottoId = gr.Key, Quantità = gr.Sum(o => o.Quantity) }
                into gr1
                join p in productList
                on gr1.ProdottoId equals p.Id
                select new { p.Name, gr1.Quantità };


            foreach (var item in lista4)
            {
                Console.WriteLine("{0} - {1}", item.Name, item.Quantità);
            }
        }
    }
}
