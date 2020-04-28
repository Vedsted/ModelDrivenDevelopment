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