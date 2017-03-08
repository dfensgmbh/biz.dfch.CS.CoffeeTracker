function Delete-Entities {
	PARAM
	(
		[Parameter(Mandatory = $true, Position = 0)]
		[ValidateNotNullOrEmpty()]
		[string] $EntityName
		,
		[Parameter(Mandatory = $true, Position = 1)]
		[ValidateNotNullOrEmpty()]
		[string] $OdataComparison
		,
		[Parameter(Mandatory = $true)]
		[ValidateNotNullOrEmpty()]
		[string] $Token
	
	)
	Process
	{
		$baseUri = "CoffeeTracker/api";

		$entityFilter = '$filter={0}' -f $OdataComparison;
		$entityGetUri = "{0}/{1}?{2}" -f $baseUri, $EntityName, $entityFilter;

		$authString = "bearer {0}" -f $Token;
		$headers = [System.Collections.Generic.Dictionary[[String],[String]]]::New();
		$headers.Add("Authorization", $authString);
	
		$entities = Invoke-RestMethod -Method Get -Uri $entityGetUri -Headers $headers;
		
		foreach ($entity in $entities.value) 
		{
			$deleteUri = "{0}/{1}({2})" -f $baseUri, $EntityName, $entity.Id;
			Invoke-RestMethod -Method Delete -Uri $deleteUri -Headers $headers;
		}

		# Check if all was deleted
		$entitiesAfterDeletion = Invoke-RestMethod -Method Get -Uri $entityGetUri -Headers $headers;

		if ($entitiesAfterDeletion.values.Count -eq 0) {
			$OutputParameter = $true;
		}
		else
		{
			$OutputParameter = $false;
		}
	}
	End 
	{
		return $OutputParameter;
	}
}

#
# Copyright 2017 d-fens GmbH
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
# http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
#
