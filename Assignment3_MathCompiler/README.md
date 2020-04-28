# Assingment # - Math Compiler
Math compiler assignment for Model-driven software development course.
The code can be found on GitHub here: [Vedsted/MathInterpreter](https://github.com/Vedsted/ModelDrivenDevelopment/tree/master/Assignment3_MathCompiler)

This README can also be found on GitHub here: [README](https://github.com/Vedsted/ModelDrivenDevelopment/blob/master/Assignment3_MathCompiler/README.md)


## Table of Contents
- [Cloning and importing project](#cloning-and-importing-project)
- [Status and Features](#status-and-features)
  - [Variables](#variables)
  - [External Input Validation](#external-input-validation)
- [Xtext file](#xtext-file)
- [Generator File](#generator-file)
- [Example App](#example-app)

# Cloning and importing project
The project can be cloned normally from github. The project is part of the [Vedsted/ModelDrivenDevelopment](https://github.com/Vedsted/ModelDrivenDevelopment) repository an can be found in the `Assignment2_MathInterpreter` folder.

Erros can occur when the project is imported into eclipse. This is due to empty folders missing in some of the projects. These errors can simply be fixed by creating empty folders where they are missing.

# Status and Features
A project has been created with the `.mc` extension. An example of how to use the language can be seen in section: [Example App](#example-app).

The language fulfills the requirements of the assignment. There is currently no known bugs in the code.

Furthermore, I have replaced the predefined `INT` with `FLOAT` in my grammer as well as add negative numbers to the language. Examples of both can be seen in either section [Variables](#variables) or [Example App](#example-app).


## Variables
Variables can be used the following ways in the DSL:
```js
let a = ((1+2)*5)*(2-1)/3   // a = 5
let b = 1+2*5*2-0/3         // b = 21

let c =                     // c = 15
	let c = 5 in 
	let d = c*2 in
	d+c
;								

let zero = 2--3+-5;         // zero = 0

let pi = 22/7               // pi = 3.142857

let x = -5+10*(1+0.5)/1.5;  // x = 5
```

## External Input Validation
Validation for the use external refrence (ExternalRef) was created. This informs the user if to few/many arguments are parsed to the external function. This is done by validating the number of inputs:

```java
@Check
	def checkNumberOfParameters(ExternalRef exRef){
		if (exRef.parameters.size != exRef.value.inputs.size){
			error(
				'''
				Number of parameters does not match external '«exRef.value.name»' definition!
				Found «exRef.parameters.size» but expected «exRef.value.inputs.size».
				''',
				MathCompilerPackage.Literals.EXTERNAL_REF__PARAMETERS,
				'INVALID_NUMBER_OF_PARAMETERS'
			)
		}	
	}
```

# Xtext file
Xtext grammar specification.
```js
grammar sdu.jvs.MathCompiler hidden(WS, ML_COMMENT, SL_COMMENT)
import "http://www.eclipse.org/emf/2002/Ecore" as ecore
generate mathCompiler "http://www.jvs.sdu/MathCompiler"

Model:
	externals += External*
	variables += Variable*
	results += Result+
;

/*******************************************
 * Custom Terminals.
 * Hidden terminals are ignored in the dsl
 */
terminal ID: '^'?('a'..'z'|'A'..'Z'|'_') ('a'..'z'|'A'..'Z'|'_'|'0'..'9')*;

terminal FLOAT returns ecore::EFloat: ('0'..'9')+'.'('0'..'9')+ | ('0'..'9')+;

terminal ML_COMMENT : '/*' -> '*/';	
terminal SL_COMMENT : '//' !('\n'|'\r')* ('\r'? '\n')?;

terminal WS         : (' '|'\t'|'\r'|'\n')+;
/********************************************/

External:
	'external' name=ID '(' inputs+=Input (',' inputs+=Input)* ')'
;

Input:
	name=ID
;

Variable:
	'let' name=ID '=' (funcVars+=Variable 'in')* expression=Expression (';')?
;

Result:
	'result' name=ID 'is' expression=Expression (';')?// Id added to display hovering
;

Expression:
	PlusOrMinus
;

PlusOrMinus returns Expression:
	MultOrDiv (({Plus.left=current} '+' | {Minus.left=current} '-' ) right=MultOrDiv)*
;

MultOrDiv returns Expression:
	Atomic (({Mult.left=current} '*' | {Div.left=current} '/' ) right=Atomic)*
;

Atomic returns Expression:
	'(' Expression ')' |
	{FloatConstant} value=FLOAT |
	{NegFloatConstant} '-' value=FLOAT |
	{VariableRef} value=[Variable] |
	{ExternalRef} value=[External]'('(parameters+=Expression (',' parameters+=Expression)*)?')'
;
```

# Generator File
Generator file used to generate the java file.

```java
class MathCompilerGenerator extends AbstractGenerator {

	override void doGenerate(Resource resource, IFileSystemAccess2 fsa, IGeneratorContext context) {
		var results = resource.allContents.toList.filter(Result)
		var externals = resource.allContents.toList.filter(External)
		
		fsa.generateFile("/mathcompiler/MathCompiler.java", genFile(results, externals))
	}
	
	def CharSequence genFile(Iterable<Result> results, Iterable<External> externals) {
		'''
		package mathcompiler;
		
		public class MathCompiler {
			
			«IF !externals.isEmpty»
			public static interface Externals {
				«FOR e : externals»
				public double «e.name»(«e.extractInputs»);
				«ENDFOR»
			}
			
			private Externals externals;
			
			public MathCompiler(Externals _externals) {
				externals = _externals;
			}
			«ENDIF»

			public void compute(){
				«FOR result : results»
				System.out.println("«result.name» = "+«result.expression.asString»);
				«ENDFOR»	
			}
		}
		'''
	}
		
	def extractInputs(External external){
		'''«FOR input: external.inputs SEPARATOR(', ')»double «input.name»«ENDFOR» '''
	}
	
	def String asString(Expression exp){
		switch exp {
			FloatConstant: exp.value + ""
			NegFloatConstant: "-"+exp.value
			VariableRef: '''(«exp.value.expression.asString»)'''
			ExternalRef: '''externals.«exp.value.name»(«FOR input:exp.parameters SEPARATOR(' ,')»«input.asString»«ENDFOR»)'''
			Plus: '''(«exp.left.asString» + «exp.right.asString»)'''
			Minus: '''(«exp.left.asString» - «exp.right.asString»)'''
			Mult: '''(«exp.left.asString» * «exp.right.asString»)'''
			Div: '''(«exp.left.asString» / «exp.right.asString»)'''
		}
	}
}
```

# Example App
Here is an example of how to use the Math Compiler language.

Example MC file:
```js
external linear(m, x, b)

let a = 5

let x = 
	let a = 2 in
	let b = -1 in
	a + b

result linear is linear(a, x, 0) * ((2--2)/4)
result MyResult is a*x+1 
```

Output from MC file:
```java
package mathcompiler;

public class MathCompiler {
	
	public static interface Externals {
		public double linear(double m, double x, double b );
	}
	
	private Externals externals;
	
	public MathCompiler(Externals _externals) {
		externals = _externals;
	}

	public void compute(){
		System.out.println("linear = "+(externals.linear((5.0) ,(((2.0) + (-1.0))) ,0.0) * ((2.0 - -2.0) / 4.0)));
		System.out.println("MyResult = "+(((5.0) * (((2.0) + (-1.0)))) + 1.0));
	}
}
```

Class using generated file:
```java
import mathcompiler.MathCompiler
import mathcompiler.MathCompiler.Externals

class Main {
	
	def static void main(String[] args) {
		var math = new MathCompiler(new Externals(){
			
			override linear(double m, double x, double b) {
				m*x+b
			}
			
		})
		
		math.compute
	}
}
```

Output when running main:
```js
linear = 5.0
MyResult = 6.0
```