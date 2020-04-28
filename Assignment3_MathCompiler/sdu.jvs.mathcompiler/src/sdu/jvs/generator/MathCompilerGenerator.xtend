/*
 * generated by Xtext 2.20.0
 */
package sdu.jvs.generator

import org.eclipse.emf.ecore.resource.Resource
import org.eclipse.xtext.generator.AbstractGenerator
import org.eclipse.xtext.generator.IFileSystemAccess2
import org.eclipse.xtext.generator.IGeneratorContext
import sdu.jvs.mathCompiler.Result
import sdu.jvs.mathCompiler.Variable
import sdu.jvs.mathCompiler.Expression
import sdu.jvs.mathCompiler.FloatConstant
import sdu.jvs.mathCompiler.NegFloatConstant
import sdu.jvs.mathCompiler.VariableRef
import sdu.jvs.mathCompiler.Plus
import sdu.jvs.mathCompiler.Minus
import sdu.jvs.mathCompiler.Mult
import sdu.jvs.mathCompiler.Div
import java.util.HashMap
import java.util.Collection
import sdu.jvs.mathCompiler.External
import sdu.jvs.mathCompiler.ExternalRef
import java.util.Collections
import java.util.ArrayList
import java.util.List

/**
 * Generates code from your model files on save.
 * 
 * See https://www.eclipse.org/Xtext/documentation/303_runtime_concepts.html#code-generation
 */
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
			
			«FOR variable : extractVariables(results)»
			private double «variable.name» = «variable.expression.asString()»;
			«ENDFOR»
			
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
	
	
	def checkResults(Iterable<Result> results){
		System.out.println('results:')
		results.forEach[r | System.out.println(r.name)]
	}
	
	/*
	 * Extracts variables used by result
	 * This results in unused variables not being included in final java code. 
	 */
	def List<Variable> extractVariables(Iterable<Result> results){
		checkResults(results)
		val variablesMap = new HashMap<String, Variable>()
		results.forEach[expression.extractVariables(variablesMap)]
		checkResults(results)
		return new ArrayList(variablesMap.values).reverse // Reversed due to variable dependencies
	}
	
	def HashMap<String, Variable> extractVariables(Expression expression, HashMap<String, Variable> v){
		switch expression {
			FloatConstant, 
			NegFloatConstant: v
			VariableRef: {
				expression.value.expression.extractVariables(v)
				v.put(expression.value.name, expression.value)
				v	
			}
			ExternalRef: {
				expression.parameters.forEach[extractVariables(v)]
				v
			}
			Plus: expression.right.extractVariables(expression.left.extractVariables(v))
			Minus: expression.right.extractVariables(expression.left.extractVariables(v))
			Mult: expression.right.extractVariables(expression.left.extractVariables(v))
			Div: expression.right.extractVariables(expression.left.extractVariables(v))
		}
	}
	
	
	
	def String asString(Expression exp){
		switch exp {
			FloatConstant: exp.value + ""
			NegFloatConstant: "-"+exp.value
			VariableRef: exp.value.name
			ExternalRef: '''externals.«exp.value.name»(«FOR input:exp.parameters SEPARATOR(' ,')»«input.asString»«ENDFOR»)'''
			Plus: '''(«exp.left.asString» + «exp.right.asString»)'''
			Minus: '''(«exp.left.asString» - «exp.right.asString»)'''
			Mult: '''(«exp.left.asString» * «exp.right.asString»)'''
			Div: '''(«exp.left.asString» / «exp.right.asString»)'''
		}
	}
}
