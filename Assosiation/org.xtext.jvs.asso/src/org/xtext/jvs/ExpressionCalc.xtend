package org.xtext.jvs

import org.xtext.jvs.asso.Expression
import org.xtext.jvs.asso.IntConstant
import org.xtext.jvs.asso.VariableRef
import org.xtext.jvs.asso.Plus
import org.xtext.jvs.asso.Minus
import org.xtext.jvs.asso.Mult
import org.xtext.jvs.asso.Div
import org.xtext.jvs.asso.InlineVariable
import org.xtext.jvs.asso.NegIntConstant

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
	/*
	def static int compute(VariableRef variableRef){
		if(variableRef.value.subExpression != null){
			variableRef.value.expression.compute
			return variableRef.value.subExpression.compute
		} else {
			return variableRef.value.expression.compute	
		}
	}
	*/
}