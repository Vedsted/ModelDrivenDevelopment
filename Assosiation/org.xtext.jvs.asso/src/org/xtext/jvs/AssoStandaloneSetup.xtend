/*
 * generated by Xtext 2.20.0
 */
package org.xtext.jvs


/**
 * Initialization support for running Xtext languages without Equinox extension registry.
 */
class AssoStandaloneSetup extends AssoStandaloneSetupGenerated {

	def static void doSetup() {
		new AssoStandaloneSetup().createInjectorAndDoEMFRegistration()
	}
}
