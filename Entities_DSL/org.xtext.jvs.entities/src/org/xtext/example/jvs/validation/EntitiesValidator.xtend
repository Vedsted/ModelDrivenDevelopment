/*
 * generated by Xtext 2.20.0
 */
package org.xtext.example.jvs.validation

import org.xtext.example.jvs.entities.EntitiesPackage
import org.xtext.example.jvs.entities.Entity
import org.eclipse.xtext.validation.Check
import static extension java.lang.Character.*
import org.xtext.example.jvs.entities.Attribute

/** 
 * This class contains custom validation rules. 
 * See https://www.eclipse.org/Xtext/documentation/303_runtime_concepts.html#validation
 */
class EntitiesValidator extends AbstractEntitiesValidator {
	
	protected static val ISSUE_CODE_PREFIX = "org.xtext.example.jvs.entities."
	
	public static val HIERARCHY_CYCLE = "HierarchyCycle"
	public static val INVALID_ENTITY_NAME = "InvalidEntityName"
	public static val INVALID_ATTRIBUTE_NAME = "InvalidAttributeName"
	
	@Check
	def checkNoCycleInEntityHierarchy(Entity entity) {
		if (entity.superType == null) return
		
		val visitedEntries = newHashSet(entity)
		
		var current = entity.superType
		
		while (current != null) {
			if (visitedEntries.contains(current)){
				error("Cycle in hierarchy of entity '"+current.name+"'", 
					EntitiesPackage.eINSTANCE.entity_SuperType,
					HIERARCHY_CYCLE, // issue code
					current.superType.name //issue data
				)
				return
			}
			visitedEntries.add(current)
			current = current.superType
		}
	}
	
	@Check
	def checkEntityNameStartsWithUppercase(Entity entity) {
		if(entity.name.charAt(0).isLowerCase){
			warning("Name of entity '" + entity.name + "' should start with capital letter", 
				EntitiesPackage.eINSTANCE.entity_Name,
				INVALID_ENTITY_NAME,
				entity.name
			)
			
		}
	}
	
	@Check
	def checkAttributeNameStartsWithLowercase(Attribute attribute) {
		if(attribute.name.charAt(0).isUpperCase){
			warning("Name of attribute '" + attribute.name + "' should start with lowercase letter", 
				EntitiesPackage.eINSTANCE.attribute_Name,
				INVALID_ATTRIBUTE_NAME,
				attribute.name
			)	
		}
	}
}
