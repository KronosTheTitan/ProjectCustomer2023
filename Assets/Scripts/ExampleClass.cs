using System;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// This class serves as an example of what good usage of the code quality guidelines looks like.
/// </summary>
public class ExampleClass : MonoBehaviour
{
    /// <summary>
    /// All constants should be in all caps seperated by _.
    /// </summary>
    public const int PUBLIC_CONST_NUMBER = 1;
    
    /// <summary>
    /// Public statics should be CamelCased.
    /// </summary>
    public static int PublicStaticNumber;

    /// <summary>
    /// Private statics should be pascalCased and start with an _
    /// </summary>
    private static int _privateStaticNumber;

    /// <summary>
    /// Public variables are pascalCased.
    /// </summary>
    public int publicNumber;

    /// <summary>
    /// Private variables are always pascalCased.
    /// </summary>
    [SerializeField] private int privateNumber;
    
    /// <summary>
    /// non serialized privates are started with an _.
    /// </summary>
    private int _privateNumber;

    /// <summary>
    /// Unity functions always go above the other functions.
    /// </summary>
    private void Start()
    {
        PublicExampleMethod();
        PrivateExampleMethod();
    }

    //If you have a lot of Get functions try to put them in a region.
    #region GetMethods
    /// <summary>
    /// When writing a get function CamelCase the name and put Get in front of the now CamelCased variable name.
    /// </summary>
    /// <returns></returns>
    public int GetPrivateNumber()
    {
        return privateNumber;
    }
    #endregion
    
    /// <summary>
    /// All Methods are CamelCased.
    /// </summary>
    public void PublicExampleMethod()
    {
        
    }
    
    /// <summary>
    /// Private methods come after all the public ones.
    /// </summary>
    private void PrivateExampleMethod()
    {
        
    }
}