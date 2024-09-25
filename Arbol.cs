using System;

public class Nodo<T>
{
    //atributos
    T dato; 
    Nodo left;
    Nodo right;

    public Nodo<T data>
    {
        dato = data;
        left = null;
        right = null;
    }

}

public class Arbol <T>
{
    //atributos
    Nodo <T> root;

    public Arbol<T>
    {
        root = null;
    }

}
