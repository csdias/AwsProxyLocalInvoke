install dotnet 5
install aws toolkit
install sam cli

dotnet new -i Amazon.Lambda.Templates::*
dotnet tool install --global Amazon.Lambda.Tools

dotnet new serverless.EmptyServerless -n DebuggingExample
cd DebuggingExample
dotnet new sln -n DebuggingExample\
dotnet sln DebuggingExample.sln add */*/*.csproj

# to test 
dotnet test

# to compile and publish
dotnet publish -c Release

# to create an input sample (on the src/DebbugingExample folder)
sam local generate-event apigateway aws-proxy > testApiRequest.json

# to invoke the lambda function locally
sam local invoke "DebuggingExampleFunction" --event testApiRequest.json --template serverless.template

# to run the api locally
sam local start-api --template serverless.template

# to create the bucket 
aws s3api create-bucket --bucket csdias-debuggin-example-bucket --region eu—west-2

# to create a package
sam package --template-file serverless.template --output-template-file output-serverless.template --s3-bucket csdias-debugging-example-bucket --region eu-west-2

# to deploy to aws
sam deploy --template-file output-serverless.template --stack-name csdias-debugging-template-stack --capabilities CAPABILITY_IAM --region eu-west-2 --s3-bucket csdias-debugging-example-bucket

# to invoke remotely
dotnet lambda invoke-function DebuggingExample -–region eu-west-2

# to bring the logs to show in the terminal
sam logs -n DebuggingExample --region eu-west-2