/*
 * generated by Xtext 2.20.0
 */
package org.xtext.example.jvs.generator

import org.eclipse.emf.ecore.resource.Resource
import org.eclipse.xtext.generator.AbstractGenerator
import org.eclipse.xtext.generator.IFileSystemAccess2
import org.eclipse.xtext.generator.IGeneratorContext
import org.xtext.example.jvs.entities.Entity
import org.xtext.example.jvs.entities.Attribute
import org.xtext.example.jvs.entities.AttributeType
import org.xtext.example.jvs.entities.ElementType
import org.xtext.example.jvs.entities.BasicType
import org.xtext.example.jvs.entities.EntityType

/**
 * Generates code from your model files on save.
 * 
 * See https://www.eclipse.org/Xtext/documentation/303_runtime_concepts.html#code-generation
 */
class EntitiesGenerator extends AbstractGenerator {

	override void doGenerate(Resource resource, IFileSystemAccess2 fsa, IGeneratorContext context) {
		var entitites = resource.allContents.toIterable.filter(Entity)
		entitites.forEach[
			entity |
			fsa.generateFile("entities/"+entity.name+".java", entity.compile)
		]
	}
		
	def CharSequence compile(Entity entity) {
		'''
		package entities;
		
		public class «entity.name»«entity.superType != null ? " extends " + entity.superType.name : ""» {
			«FOR attribute : entity.attributes»
				private «attribute.type.compile» «attribute.name»; 
			«ENDFOR»
			
			public «entity.name»(){
				«entity.superType != null ? "super();":""»
			}
			
			«FOR attribute : entity.attributes»
			public «attribute.type.compile» get«attribute.name.toFirstUpper»(){
				return this.«attribute.name»;
			}
			
			public void set«attribute.name.toFirstUpper»(«attribute.type.compile» «attribute.name»){
				this.«attribute.name» = «attribute.name»;
			}
			«ENDFOR»
				
		}
		'''
	}
	
	def compile(AttributeType attributeType){
		attributeType.elementType.typeToString + (attributeType.array ? "[]": "")
	}
		
	def dispatch typeToString(BasicType type) {
		type.typeName == "string" ? "String": type.typeName
	}
	
	def dispatch typeToString(EntityType type) {
		type.entity.name
	}
		
}