function CRUD-User{
	PARAM
	(
		[Parameter(Mandatory = $true, Position = 0)]
		[string] $UserName
		,
		[Parameter(Mandatory = $false, ParameterSetName = 'Update')]
		[string] $NewUserName
		,
		[Parameter(Mandatory = $true, ParameterSetName = 'Create')]
		[Parameter(Mandatory = $false, ParameterSetName = 'Update')]
		[string] $Password
		,
		[Parameter(Mandatory = $false, ParameterSetName = 'Create')]
		[switch] $Create
		,
		[Parameter(Mandatory = $true, ParameterSetName = 'Update')]
		[Parameter(Mandatory = $true, ParameterSetName = 'Read')]
		[string] $Token
		,
		[Parameter(Mandatory = $false, ParameterSetName = 'Update')]
		[switch] $Update
		,
		[Parameter(Mandatory = $false, ParameterSetName = 'Delete')]
		[switch] $Delete
		,
		[Parameter(Mandatory = $false, ParameterSetName = 'Read')]
		[switch] $Read
	)
	Process
	{
		$uri = "CoffeeTracker/api/Users";

		if($Token)
		{
			$authString = "bearer {0}" -f $Token;
			$headers = [System.Collections.Generic.Dictionary[[String],[String]]]::New();
			$headers.Add("Authorization", $authString);
		}
			
		if($PSCmdlet.ParameterSetName -eq 'Create')
		{
			$userBody = @{
				Name = $UserName
				Password = $Password
			}

			$result = Invoke-RestMethod -Method Post -Uri $uri -Body $userBody
		}
		elseif($PSCmdlet.ParameterSetName -eq 'Update')
		{
			$filter = '$filter';
			$getUri = "{0}?$filter=Name eq '{1}'" -f $uri, $UserName;

			
			$userResult = Invoke-RestMethod -Method Get -Uri $getUri -Headers $headers;
			$user = $userResult.value;

			if($NewUserName)
			{
				$userBody = @{
					Name = $NewUserName
					Password = $user.Password
				}
				
			}
			else
			{
				$userBody = @{
					Name = $UserName
					Password = $user.Password
				}
			}
			
			if($Password)
			{
				$userBody["Password"] = $Password;
			}

			$headers.Add("Content-Type", "application/json;odata=verbose")
			$updateUri = "$uri({0})" -f $user.Id;

			$userBodyJson = $userBody | ConvertTo-Json;

			$result = Invoke-RestMethod -Method Put -Uri $updateUri -Body $userBodyJson -Headers $headers;
		}
		elseif($PSCmdlet.ParameterSetName -eq 'Delete')
		{
			. .\Delete-Entities.ps1;
			$result = Delete-Entities -EntityName "Users" -ODataComparison "Name eq '$UserName'";
		}
		elseif($PSCmdlet.ParameterSetName -eq 'Read')
		{
			$filter = '$filter';
			$getUri = "{0}?$filter=Name eq '{1}'" -f $uri, $UserName;
			$response = Invoke-RestMethod -Method Get -Uri $getUri -Headers $headers;
			$result = $response.value;
		}

		$OutputParameter = $result;
	}
	End
	{
		return $OutputParameter;
	}
}
#
# Copyright $year$ d-fens GmbH
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
