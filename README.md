# Cómo crear un plugin para uAdventure
En esta guía veremos paso a paso todo lo necesario para la inserción de un plugin en uAdventure para añadir una funcionalidad.

## Antes de empezar

uAdventure es una herramienta diseñada para crear juegos serios usando la tecnología Unity 3D. En esta guía no se explica cómo utilizar uAdventure, si no la posibilidad de añadir una nueva funcionalidad a esta herramienta mediante la inserción de un plugin. Para hacer más comprensible esta guía, se usará "Example" como modelo en todos los nombres del proyecto.

## Requisitos indispensables

Para la creación de un plugin en uAdventure es necesario tener instalado Unity y un proyecto de uAdventure.


## Paso a paso
* Crea una carpeta con el nombre del módulo que quieras añadir en la que a su vez haya carpetas que se llamen **Model**, **Editor**, **Scripts** y **Resources**.
La ruta de la nueva carpeta debe ser la siguiente:
```
uAdventure/Assets/Example
```

* En la carpeta **Model** debe haber dos ficheros:
1) Example: Esta clase contendrá todos los datos necesarios para el plugin, típicamente Id, Documentation, Effects y Conditions, pero debes añadir todo lo específico para tu módulo, por ejemplo, un texto a mostrar, la ruta de una imagen de fondo, etc..
2) ExampleEffect: En esta clase debe implementar la clase AbstractEffect. El método implementado 'getType' será de este modo:
```
public override EffectType getType(){
		return EffectType.CUSTOM_EFFECT;
	}
```

* La carpeta **Editor** tendrá cuatro archivos:
1) ExampleWriter: En esta clase se implementa la escritura del objeto Exampe en el archivo XML. Debe heredar de la clase ParametrizedDOMWriter y registrar la etiqueta \[DOMWriter(typeof(Example))\]. Además aquí se decide el nombre de la etiqueta que buscará el Parser:
```
protected override string GetElementNameFor(object target){
		return "Example";
	}
```
2) ExampleEffectWriter: De igual modo pasa con el efecto en esta clase. Usando ExampleEffect en lugar de Example.
3) ExampleEffectEditor: Aquí se implementa la opción para que este tipo de efecto pueda ser escogido en una escena. Debe heredar de EffectEditor. 
4) ExampleWindowExtension: Esta clase mostrará la vista del editor propia del módulo Example. Debe tener la etiqueta \[EditorWindowExtension(x, typeof(Minigame))\] (donde x es la prioridad en el menú de componentes), y heredar de la clase ReorderableListEditorWindowExtension.

* La carpeta **Scripts** tendrá al menos tres archivos:
1) ExampleParser: Esta clase se encarga de obtener la información del objeto Example desde el archivo XML de configuración. Además aquí se decide el nombre de la etiqueta que buscará el Parser. Debe heredar de la clase IDOMParser y registrar las etiquetas \[DOMParser(typeof(Example))\] y \["Example"\] (esta última debe coincidir exactamente con el valor devuelto en el método GetElementNameFor de ExampleWriter).
2) ExampleEffectParser: De igual modo pasa con el efecto en esta clase. Usando ExampleEffect en lugar de Example.
3) ExampleEffectRunner: Se encargará de ejecutar la lógica que conlleve el plugin. Debe contener la etiqueta \[CustomEffectRunner(typeof(MinigameEffect))\] y heredar de CustomEffectRunner. Es importante tener en cuenta que no se pueden crear GameObjects de tipo Monobehaviour desde esta clase, pero si es posible cargar uno ya existente:
```
Resources.Load<GameObject>("ExamplePrefab");
```

Además, en este directorio se almacenarán todos los scripts necesarios para la ejecución del plugin.

* Opcionalmente habrá una carpeta Resources en la que almacenar todo el contenido multimedia o auxiliar como imágenes, audios, o el icono que representará al plugin.


## Instalación

Para instalar un módulo en uAdventure bastará con copiar la carpeta 'Example' en el interior del directorio Assets de uAdventure.
