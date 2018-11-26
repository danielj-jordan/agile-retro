pipeline {
    agent { 
      dockerfile {
        filename 'scripts/ci-build.dockerfile'
        additionalBuildArgs '-t app-image '
        args '-u root --privileged --shm-size="128m" -it -p 9222:9222 -p 5000:5000 -p 4200:4200 -p 27017:27017 -p 127.0.0.1:8022:22'
      } 
    }
    stages {

      stage('Build'){
          environment {
            PATH="/usr/local/bin:/Applications/Docker.app/Contents/Resources/bin:$PATH"
          }
          steps {
            sh 'ls /agile-retro'
            sh 'dotnet --version'
            sh 'whoami'
            sh 'dotnet build /agile-retro/app/app.csproj'
            sh 'ng --version'
            sh 'ls -al /agile-retro/client'
            sh 'cd /agile-retro/client && npm install && ng build'
            
          }
      }
      stage('Test'){
        steps{
           sh 'mkdir /agile-retro/data'
           sh '/mongodb/bin/mongod --dbpath "/agile-retro/data"  --bind_ip 0.0.0.0  &'
           sh 'dotnet test /agile-retro/Retrospective.Data.Test/Retrospective.Data.Test.csproj'
           //sh 'dotnet test /agile-retro/Retrospective.Domain.Test/Retrospective.Domain.Test.csproj'
           //sh 'dotnet test /agile-retro/apptest/apptest.csproj'
           //sh 'dotnet run -p /agile-retro/tools/testdata/testdata.csproj'
           sh 'cd /agile-retro/client && ng test  --watch=false'
        }
      }
    }
}