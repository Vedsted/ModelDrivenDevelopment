

import mathcompiler.MathCompiler
import mathcompiler.MathCompiler.Externals

class Main {
	
	def static void main(String[] args) {
		var math = new MathCompiler(new Externals{
			
			override linear(double x, double a, double b) {
				a*x+b
			}
			
		})
		
		math.compute
	}
}