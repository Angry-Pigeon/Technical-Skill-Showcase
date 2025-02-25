using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASoliman.Utils.EditableRefs.Demo
{
    public class ExampleComponent : MonoBehaviour
    {
        [EditableRef] public ExampleClass exampleClass;
        [EditableRef] public ExampleData exampleData;
    }
}
