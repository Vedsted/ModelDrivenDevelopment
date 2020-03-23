# Assingment 2 - Math Interpreter

## Xtext file
```xtext
Model:
	variables += Variable*
	evaluations += EvalExpression*
;

Expression:
	PlusOrMinus
;

Atomic returns Expression:
	'(' Expression ')' |
	{IntConstant} value=INT |
	{NegIntConstant} '-' value=INT |
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
	'let' name=ID '=' expression=Expression ('in' subExpression=Expression)?
;

EvalExpression:
	'eval' name=ID '=' expression=Expression // Id added to display hovering
;
```

## Generator File
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
			JOptionPane.showMessageDialog(null, "result = "+result,"Math Language", JOptionPane.INFORMATION_MESSAGE)
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

## Hovering - AssoLabelProvider
The class `AssoLabelProvider` is used to show the result of the expressions via hovering. The result is shown when the user keeps the mouse over the expression id.

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

## Expression Calculator - ExpressionCalc
To provide calculations for both the generator and the hovering feature, the expression calculation was extracted to the `ExpressionCalc.xtend` file.

```java
class ExpressionCalc {
	
	def static int compute(Expression expression){
		switch expression {
			IntConstant: expression.value
			NegIntConstant: expression.value * -1
			InlineVariable: expression.value.expression.compute
			VariableRef: expression.compute
			Plus : expression.left.compute() + expression.right.compute()
			Minus : expression.left.compute() - expression.right.compute()
			Mult: expression.left.compute() * expression.right.compute()
			Div: expression.left.compute() / expression.right.compute()
			default: throw new Error("in method: compute(Expression expression)")
		}
	}
}
```
