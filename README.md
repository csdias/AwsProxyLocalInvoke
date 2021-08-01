
# to compile and publish
dotnet publish -c Release

# to create an input sample (on the src/DebbugingExample folder)
sam local generate-event apigateway aws-proxy > testApiRequest.json

# to invoke the lambda function locally
sam local invoke "DebuggingExampleFunction" --event testApiRequest.json --template serverless.template

# to run the api locally
sam local start-api --template serverless.template

# to create a package
sam package --template-file serverless.template --output-template-file debugging-serverless.template --s3-bucket debugging-example-bucket --region eu-west-2

  # to deploy to aws
  sam deploy --template-file debugging-serverless.template --stack-name DebuggingExample --capabilities CAPABILITY_IAM --region eu-west-2

# to invoke remotely
dotnet lambda invoke-function DebuggingExample -–region eu-west-2