# Fitbit Setup

WORK IN PROGRESS - CONSTRUCTION_SITE/BAUSTELLE



[TODO]

*	replace one App with 2:
	*	Server app
	
		https://dev.fitbit.com/docs/basics/#server
		
	*	Client app
	
		https://dev.fitbit.com/docs/basics/#client
		
	*	private
	
		https://dev.fitbit.com/docs/basics/#private
		
	
Check:

*	seems like Fitbit requires implementation according to (latest) RFC

	fitbit seems to want a `Authorization: Basic xxxxx` header for the 
	access token request?		
	
	when they do it’s always Base64Encode(clientId + “:” + clientsecret)
	
	