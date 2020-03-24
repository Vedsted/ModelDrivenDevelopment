# Assingment 2 - Math Interpreter
Math interpreter assignment for Model-driven software development course.
The code can be found on GitHub here: [Vedsted/MathInterpreter](https://github.com/vedsted/modeldrivendevelopment/)

This README can also be found on GitHub here: 


## Table of Contents
- [Assingment 2 - Math Interpreter](#assingment-2---math-interpreter)
- [Cloning and importing project](#cloning-and-importing-project)
- [Status and Features](#status-and-features)
	- [Hovering](#hovering)
	- [Custom Base Grammar](#custom-base-grammar)
	- [Variables](#variables)
- [Xtext file](#xtext-file)
- [Generator File](#generator-file)
- [Hovering - AssoLabelProvider](#hovering---assolabelprovider)
- [Expression Calculator - ExpressionCalc](#expression-calculator---expressioncalc)
- [Math file](#math-file)

# Cloning and importing project
The project can be cloned normally from github. The project is part of the [Vedsted/ModelDrivenDevelopment](https://github.com/vedsted/modeldrivendevelopment) repository an can be found in the `Assignment2_MathInterpreter` folder.

Erros can occur when the project is imported into eclipse. This is due to empty folders missing in some of the projects. These errors can simply be fixed by creating empty folders where they are missing.

# Status and Features
A project has been created with the `.math` extension. An example of how to use the language can be seen in section: [Math file](#math-file).

The language fulfills the requirements of the assignment. The only known bugs that have not been fix yet is:  
1. Variables defined as: `let x = 1 +x` will result in an error. This is due to the program trying to compute `x` recursively until a StackOverflowError is thrown. This should be fixed by implementing scoping.

Furthermore, I have replaced the predefined `INT` with `FLOAT` in my grammer as well as negative numbers. Examples of both can be seen in either section [Variables](#variables) or [Math file](#math-file).


## Hovering
Hovering was implemented for variables in the `AssoLabelProvider` class in the `org.xtext.jvs.ui.labeling` package. This was done with the following function:
```
def text(Variable variable) {
	variable.name + " = " + variable.expression.compute
}
```

The compute method is defined in the `ExpressionCalc` class in the `org.xtext.jvs.util` package. The implementation can be seen in section: [Expression Calculator - ExpressionCalc](##-Expression-Calculator---ExpressionCalc).

Evalulations is still printed in the console and displayed in a pop up box.

## Custom Base Grammar
In order to get precise calculations, the INT values was changed to FLOAT's. In order to do this, the FLOAT had to be defined and the INT removed.
This was done by removing the extension of the Xtext common terminals.

```java
// Before
grammar org.xtext.jvs.Asso with org.eclipse.xtext.common.Terminals

// After
grammar org.xtext.jvs.Asso
```

This removed the predefined terminal rules. I then defined the types that was needed in the language. The following terminals was defined identically to the common terminals: `ID`, `STRING`, `ML_COMMENT`, `SL_COMMENT` and `WS`. The terminal FLOAT was defined as follows:

```
terminal FLOAT returns ecore::EFloat: ('0'..'9')+'.'('0'..'9')+ | ('0'..'9')+;
```
This definitioin of FLOAT means that it can be written in the DSL with or without decimals: `1` OR `1.0`.

Furthermore, the terminals that was defined as hidden in the common terminals was defined as hidden again. 
```
grammar org.xtext.jvs.Asso hidden(WS, ML_COMMENT, SL_COMMENT)
```

The final result can be seen in the grammar specification in section: [Xtext file](#xtext-file)


## Variables
Variables can be used the following ways in the DSL:
```js
let a = ((1+2)*5)*(2-1)/3 	// a = 5
let b = 1+2*5*2-0/3 		// b = 21

let c = 					// c = 15
	let c = 5 in 
	let d = c*2 in
	d+c
;								

let d = a*2 - (let t = a+a) // d = 0 

let zero = 2--3+-5;			// zero = 0

let pi = 22/7				// pi = 3.142857

let x = -5+10*(1+0.5)/1.5; 	// x = 5
```




# Xtext file
Xtext grammar specification.
```js
grammar org.xtext.jvs.Asso hidden(WS, ML_COMMENT, SL_COMMENT) // Custom Terminals
import "http://www.eclipse.org/emf/2002/Ecore" as ecore
generate asso "http://www.xtext.org/jvs/Asso"


Model:
	variables += Variable*
	evaluations += EvalExpression*
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

Expression:
	PlusOrMinus
;

Atomic returns Expression:
	'(' Expression ')' |
	{FloatConstant} value=FLOAT |
	{NegFloatConstant} '-' value=FLOAT |
	{VariableRef} value=[Variable]|
	{InlineVariable} '(' value=Variable ')'
;

PlusOrMinus returns Expression:
	MultOrDiv (({Plus.left=current} '+' | {Minus.left=current} '-' ) right=MultOrDiv)*
;

MultOrDiv returns Expression:
	Atomic (({Mult.left=current} '*' | {Div.left=current} '/' ) right=Atomic)*
;

Variable:
	'let' name=ID '=' (subVar+=Variable 'in')* expression=Expression (';')?
;

EvalExpression:
	'eval' expression=Expression (';')?// Id added to display hovering
;
```

# Generator File
The generator file is currently used to print the variables and expression evaluations to the console. The Generator also creates a pop up for the expression evaluations when the .math file is saved.

The `compute` method is defined in the `ExpressionCalc.xtend` file.

```java
class AssoGenerator extends AbstractGenerator {

	override void doGenerate(Resource resource, IFileSystemAccess2 fsa, IGeneratorContext context) {
		resource.allContents.filter(Variable).forEach[prettyPrint]
		resource.allContents.filter(EvalExpression).forEach[
			e | 
			e.prettyPrint // Print to console
			var result = e.expression.compute
			JOptionPane.showMessageDialog(null, 
						"result = "+result,"Math Language", 
						JOptionPane.INFORMATION_MESSAGE)
		]
	}	
		
	def void prettyPrint(Variable variable){
		System.out.println("let " + variable.name + " = " + variable.expression.toStringRep)			
	} 
	
	def void prettyPrint(EvalExpression ee){
		System.out.println("Expression = " + ee.expression.toStringRep)			
	} 
	
	def String toStringRep(Expression expression){
		switch expression {
			IntConstant: expression.value + ""
			NegIntConstant: "-"+expression.value
			VariableRef: expression.value.name
			Plus: '''(«expression.left.toStringRep» + «expression.right.toStringRep»)'''
			Minus: '''(«expression.left.toStringRep» - «expression.right.toStringRep»)'''
			Mult: '''(«expression.left.toStringRep» * «expression.right.toStringRep»)'''
			Div: '''(«expression.left.toStringRep» / «expression.right.toStringRep»)'''
		}
	}
}
```

# Hovering - AssoLabelProvider
The class `AssoLabelProvider` is used to show the result of the expressions via hovering. The result is shown when the user hovers the mouse over the expression id.

The `compute` method for expressions used in the file is defined in the `ExpressionCalc.xtend` file.

```java
class AssoLabelProvider extends DefaultEObjectLabelProvider {

	@Inject
	new(AdapterFactoryLabelProvider delegate) {
		super(delegate);
	}

	def text(Variable variable) {
		variable.name + " = " + variable.expression.compute
	}

	def text(EvalExpression ee) {
		'''Result is: «ee.name» = «ee.expression.compute»'''
	}
}
```

# Expression Calculator - ExpressionCalc
To provide calculations for both the generator and the hovering feature, the expression calculation was extracted to the `ExpressionCalc.xtend` file.

```java
class ExpressionCalc {
	def static float compute(Expression expression){
		switch expression {
			FloatConstant: expression.value
			NegFloatConstant: expression.value * -1
			VariableRef: expression.value.expression.compute
			Plus : expression.left.compute() + expression.right.compute()
			Minus : expression.left.compute() - expression.right.compute()
			Mult: expression.left.compute() * expression.right.compute()
			Div: expression.left.compute() / expression.right.compute()
			default: throw new Error("in method: compute(Expression expression)")
		}
	}
}
```

# Math file
Here is an example of how to use the Math language.
```js
let a = ((1+2)*5)*(2-1)/3 	// a = 5
let b = 1+2*5*2-0/3 		// b = 21

let c = 					// c = 15
	let c = 5 in 
	let d = c*2 in
	d+c
;								

let e = -5.5;				// e = -5.5

let zero = 2--3+-5;			// zero = 0

let pi = 22/7				// pi = 3.142857

let x = -5+10*(1+0.5)/1.5; 	// x = 5

/* Eval is displayed in a pop up */
eval (b*43+(2/14))*zero 	// -> 0
```