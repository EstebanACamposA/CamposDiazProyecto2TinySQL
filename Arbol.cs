using System;

public class Fila : IComparable<Fila>
{
    public int ID { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }

    public Fila(int id, string nombre, string apellido)
    {
        ID = id;
        Nombre = nombre;
        Apellido = apellido;
    }

    // Implementamos IComparable para que se pueda comparar por ID (o puedes cambiar el criterio)
    public int CompareTo(Fila other)
    {
        return this.ID.CompareTo(other.ID);  // Comparación por ID
    }

    public override string ToString()
    {
        return $"ID: {ID}, Nombre: {Nombre}, Apellido: {Apellido}";
    }
}

public class Nodo
{
    //atributos
    Fila dato; 
    Nodo left;
    Nodo right;

    public Nodo(Fila data)
    {
        dato = data;
        left = null;
        right = null;
    }

}

public class Arbol 
{
    //atributos
    Nodo root;

    public Arbol
    {
        root = null; //crea el arbol vacio
    }

    public void Insertar(Fila fila)
    {
        root = InsertarRecursivo(root, fila) //asigna la raiz a un metodo recursivo
    }

    private Nodo InsertarRecursivo(Nodo nodo, Fila fila)
    {
        if (nodo == null) //si el arbol esta vacio o si no se puede bajar mas en el arbol, el nodo se vuelve nulo y se le asigna el valor
        {
            nodo = new Nodo(fila);
            return fila;
        }

        if (fila.CompareTo(nodo.data) < 0) //CompareTo devuelve un -1 si el elemento es menor al que se esta comparando
        {
            nodo.Izquierdo = InsertarRecursivo(nodo.Izquierdo, fila); //como es menor, desciende al hijo izquierdo y verifica recusrivamente otra vez.
        }

        else if (fila.CompareTo(nodo.Data) > 0) //CompareTo devuelve un 1 si el elemento es mayor
        {
            nodo.Derecho = InsertarRecursivo(nodo.Derecho, fila); // como es mayor, desciende al hijo derecho y verifica recusrivamente otra vez
        }
    }

}
