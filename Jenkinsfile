pipeline {  
 agent any  
 environment {  
  dotnet = 'C:\\Program Files\\dotnet\\dotnet.exe' 
 }  
 stages {  
  stage('Checkout') {  
   steps {
       git credentialsId: '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', url: 'https://github.com/ManavNanda1/SuperariLife_Demo.git', branch: 'main'
   }  
  }
  stage('Build') {  
   steps {  
    bat 'dotnet build %WORKSPACE%\\SuperariLife_AdminPortalAPI.sln --configuration Release' 
    
   }  
  }
 
	stage("Release"){
      steps {
      bat 'dotnet build  %WORKSPACE%\\SuperariLife_AdminPortalAPI.sln /p:PublishProfile=" %WORKSPACE%\\TOMApi\\Properties\\PublishProfiles\\CI_CD_FolderProfile.pubxml" /p:Platform="Any CPU" /p:DeployOnBuild=true /m'
    }
  }

  stage('Deploy') {
    steps {
        
    //Pool Recycle    
    bat '%systemroot%\\System32\\inetsrv\\appcmd stop apppool /apppool.name:SuperariLife'
    // Additional steps or waiting period if needed
    bat '%systemroot%\\System32\\inetsrv\\appcmd start apppool /apppool.name:SuperariLife'

    // Stop IIS
     bat '%systemroot%\\System32\\inetsrv\\appcmd stop site /site.name:"SuperariLife"'
     
    bat '%systemroot%\\System32\\inetsrv\\appcmd start site /site.name:"SuperariLife"'
    
    bat '%systemroot%\\System32\\inetsrv\\appcmd stop site /site.name:"SuperariLife"'
    
    
     writeFile file: 'app_offline.htm', text: 'Site is under maintenance'

    
    // Deploy package to IIS
    
    bat '"C:\\Program Files (x86)\\IIS\\Microsoft Web Deploy V3\\msdeploy.exe" -verb:sync -source:contentPath="C:\\Users\\sit315.SIT\\Desktop\\IISDemo\\SuperariLife_Demo\\SuperariLife_AdminPortalAPI\\bin\\Release\\net7.0\\publish" -dest:contentPath="SuperariLife" -enableRule:DoNotDeleteRule'
    
    // Start IIS again
    bat '%systemroot%\\System32\\inetsrv\\appcmd start site /site.name:"SuperariLife"'
    }
 }
 }  
} 



