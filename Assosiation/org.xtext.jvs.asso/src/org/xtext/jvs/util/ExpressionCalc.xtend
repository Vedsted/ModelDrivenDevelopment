package org.xtext.jvs.util

import org.xtext.jvs.asso.Expression
import org.xtext.jvs.asso.FloatConstant
import org.xtext.jvs.asso.VariableRef
import org.xtext.jvs.asso.Plus
import org.xtext.jvs.asso.Minus
import org.xtext.jvs.asso.Mult
import org.xtext.jvs.asso.Div
import org.xtext.jvs.asso.InlineVariable
import org.xtext.jvs.asso.NegFloatConstant

class ExpressionCalc {
	
	def static float compute(Expression expression){
		switch expression {
			FloatConstant: expression.value
			NegFloatConstant: expression.value * -1
			InlineVariable: expression.value.expression.compute
			VariableRef: expression.value.expression.compute
			Plus : expression.left.compute() + expression.right.compute()
			Minus : expression.left.compute() - expression.right.compute()
			Mult: expression.left.compute() * expression.right.compute()
			Div: expression.left.compute() / expression.right.compute()
			default: throw new Error("in method: compute(Expression expression)")
		}
	}
}