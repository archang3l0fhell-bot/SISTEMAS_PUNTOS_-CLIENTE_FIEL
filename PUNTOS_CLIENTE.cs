using System;

namespace SistemaPuntos
{
    class Program
    {
        // Vectores paralelos para almacenar clientes
        static string[] cedulas = new string[100];
        static string[] nombres = new string[100];
        static int[] puntos = new int[100];
        static int totalClientes = 0;

        // Vectores para premios
        static string[] nombrePremios = { "Bono $10.000", "Descuento 20%", "Producto gratis" };
        static int[] puntosRequeridos = { 50, 100, 200 };
        static int[] stockPremios = { 10, 5, 3 };

        static void Main(string[] args)
        {
            int opcion;
            do
            {
                MostrarMenu();
                opcion = int.Parse(Console.ReadLine());

                switch (opcion)
                {
                    case 1: RegistrarCliente(); break;
                    case 2: RegistrarCompra(); break;
                    case 3: RedimirPremio(); break;
                    case 4: ReporteClientes(); break;
                    case 5: ReporteFieles(); break;
                    case 6:
                        Console.Write("Ingrese cédula a buscar: ");
                        string ced = Console.ReadLine();
                        int pos = BuscarCliente(ced);
                        if (pos != -1)
                            Console.WriteLine($"Cliente: {nombres[pos]} | Puntos: {puntos[pos]}");
                        else
                            Console.WriteLine("Cliente no existe");
                        break;
                    case 7: Console.WriteLine("Saliendo del sistema..."); break;
                    default: Console.WriteLine("Opción inválida"); break;
                }
                Console.WriteLine("\nPresione una tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
            } while (opcion != 7);
        }

        // Módulo | Tipo | Función
        static void MostrarMenu() // Procedimiento | Imprime las opciones del menú principal
        {
            Console.WriteLine("=== SISTEMA DE PUNTOS ===");
            Console.WriteLine("1. Registrar Cliente");
            Console.WriteLine("2. Registrar Compra");
            Console.WriteLine("3. Redimir Premio");
            Console.WriteLine("4. Reporte de Clientes");
            Console.WriteLine("5. Reporte de Clientes Fieles");
            Console.WriteLine("6. Buscar Cliente");
            Console.WriteLine("7. Salir");
            Console.Write("Seleccione una opción: ");
        }

        static void RegistrarCliente() // Procedimiento | Pide datos y guarda un nuevo cliente en los vectores
        {
            if (totalClientes >= 100)
            {
                Console.WriteLine("Límite de clientes alcanzado");
                return;
            }

            Console.Write("Cédula: ");
            string cedula = Console.ReadLine();

            if (BuscarCliente(cedula) != -1)
            {
                Console.WriteLine("El cliente ya está registrado");
                return;
            }

            Console.Write("Nombre: ");
            string nombre = Console.ReadLine();

            cedulas[totalClientes] = cedula;
            nombres[totalClientes] = nombre;
            puntos[totalClientes] = 0;
            totalClientes++;

            Console.WriteLine("Cliente registrado con éxito");
        }

        static void RegistrarCompra() // Procedimiento | Busca cliente, pide valor y suma puntos usando CalcularPuntos()
        {
            Console.Write("Cédula del cliente: ");
            string cedula = Console.ReadLine();
            int pos = BuscarCliente(cedula);

            if (pos == -1)
            {
                Console.WriteLine("Cliente no existe");
                return;
            }

            Console.Write("Valor de la compra: ");
            int valor = int.Parse(Console.ReadLine());

            if (valor <= 0)
            {
                Console.WriteLine("Valor inválido");
                return;
            }

            int puntosGanados = CalcularPuntos(valor);
            puntos[pos] += puntosGanados;

            Console.WriteLine($"Compra registrada. Puntos ganados: {puntosGanados}");
            Console.WriteLine($"Total puntos del cliente: {puntos[pos]}");
        }

        static int CalcularPuntos(int valor) // Función | Retorna valor / 1000
        {
            return valor / 1000; // División entera: 1 punto por cada $1000
        }

        static void RedimirPremio() // Procedimiento | Valida puntos y stock, descuenta ambos si es válido
        {
            Console.Write("Cédula del cliente: ");
            string cedula = Console.ReadLine();
            int pos = BuscarCliente(cedula);

            if (pos == -1)
            {
                Console.WriteLine("Cliente no existe");
                return;
            }

            Console.WriteLine("\n--- PREMIOS DISPONIBLES ---");
            for (int i = 0; i < nombrePremios.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {nombrePremios[i]} | Puntos: {puntosRequeridos[i]} | Stock: {stockPremios[i]}");
            }

            Console.Write("Seleccione el código del premio: ");
            int codPremio = int.Parse(Console.ReadLine()) - 1;

            if (codPremio < 0 || codPremio >= nombrePremios.Length)
            {
                Console.WriteLine("Código de premio inválido");
                return;
            }

            if (stockPremios[codPremio] <= 0)
            {
                Console.WriteLine("Sin stock disponible");
                return;
            }

            if (puntos[pos] < puntosRequeridos[codPremio])
            {
                Console.WriteLine($"Puntos insuficientes. Necesita: {puntosRequeridos[codPremio]}, Tiene: {puntos[pos]}");
                return;
            }

            puntos[pos] -= puntosRequeridos[codPremio];
            stockPremios[codPremio]--;

            Console.WriteLine($"¡Premio redimido con éxito! {nombrePremios[codPremio]}");
            Console.WriteLine($"Puntos restantes: {puntos[pos]}");
        }

        static void ReporteClientes() // Procedimiento | Recorre vectores y muestra todos los clientes con puntos
        {
            Console.WriteLine("\n=== REPORTE DE TODOS LOS CLIENTES ===");
            if (totalClientes == 0)
            {
                Console.WriteLine("No hay clientes registrados");
                return;
            }

            for (int i = 0; i < totalClientes; i++)
            {
                Console.WriteLine($"Cédula: {cedulas[i]} | Nombre: {nombres[i]} | Puntos: {puntos[i]}");
            }
        }

        static void ReporteFieles() // Procedimiento | Muestra solo clientes con puntos > 50
        {
            Console.WriteLine("\n=== REPORTE CLIENTES FIELES (>50 puntos) ===");
            bool hayFieles = false;

            for (int i = 0; i < totalClientes; i++)
            {
                if (puntos[i] > 50)
                {
                    Console.WriteLine($"Cédula: {cedulas[i]} | Nombre: {nombres[i]} | Puntos: {puntos[i]}");
                    hayFieles = true;
                }
            }

            if (!hayFieles)
                Console.WriteLine("No hay clientes fieles aún");
        }

        static int BuscarCliente(string cedula) // Función | Retorna la posición del cliente en el vector o -1 si no existe
        {
            for (int i = 0; i < totalClientes; i++)
            {
                if (cedulas[i] == cedula)
                    return i;
            }
            return -1;
        }
    }
}
