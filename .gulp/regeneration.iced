
###############################################
# LEGACY 
# Instead: have bunch of configuration files sitting in a well-known spot, discover them, feed them to AutoRest, done.

path = require("path");
repositoryLocalRoot = path.dirname(__dirname);

regenExpected = (opts,done) ->
  outputDir = if !!opts.outputBaseDir then "#{opts.outputBaseDir}/#{opts.outputDir}" else opts.outputDir
  keys = Object.getOwnPropertyNames(opts.mappings)
  instances = keys.length

  for kkey in keys
    optsMappingsValue = opts.mappings[kkey]
    key = kkey.trim();
    
    swaggerFiles = (if optsMappingsValue instanceof Array then optsMappingsValue[0] else optsMappingsValue).split(";")
    args = [
      "--#{opts.language}",
      "--clear-output-folder",
      "--output-folder=#{outputDir}/#{key}",
      "--license-header=#{if !!opts.header then opts.header else 'MICROSOFT_MIT_NO_VERSION'}",
      "--enable-xml"
    ]

    for swaggerFile in swaggerFiles
      args.push("--input-file=#{if !!opts.inputBaseDir then "#{opts.inputBaseDir}/#{swaggerFile}" else swaggerFile}")

    if (opts.addCredentials)
      args.push("--#{opts.language}.add-credentials=true")

    if (opts.azureArm)
      args.push("--#{opts.language}.azure-arm=true")

    if (opts.fluent)
      args.push("--#{opts.language}.fluent=true")
    
    if (opts.syncMethods)
      args.push("--#{opts.language}.sync-methods=#{opts.syncMethods}")
    
    if (opts.flatteningThreshold)
      args.push("--#{opts.language}.payload-flattening-threshold=#{opts.flatteningThreshold}")

    if (opts.generatePackageJson?)
      args.push("--#{opts.language}.generate-package-json=#{opts.generatePackageJson}")

    if (opts.generateReadmeMd?)
      args.push("--#{opts.language}.generate-readme-md=#{opts.generateReadmeMd}")

    if (opts.generateLicenseTxt?)
      args.push("--#{opts.language}.generate-license-txt=#{opts.generateLicenseTxt}")

    if (opts.sourceCodeFolderPath?)
      args.push("--#{opts.language}.source-code-folder-path=\'#{opts.sourceCodeFolderPath}\'")

    if (opts.packageName)
      args.push("--#{opts.language}.package-name=#{opts.packageName}")

    if (opts.packageVersion)
      args.push("--#{opts.language}.package-version=#{opts.packageVersion}")

    if (!!opts.nsPrefix)
      if (optsMappingsValue instanceof Array && optsMappingsValue[1] != undefined)
        args.push("--#{opts.language}.namespace=#{optsMappingsValue[1]}")
      else
        args.push("--#{opts.language}.namespace=#{[opts.nsPrefix, key.replace(/\/|\./, '')].join('.')}")

    if (opts['override-info.version'])
      args.push("--override-info.version=#{opts['override-info.version']}")
    if (opts['override-info.title'])
      args.push("--override-info.title=#{opts['override-info.title']}")
    if (opts['override-info.description'])
      args.push("--override-info.description=#{opts['override-info.description']}")

    args.push("--use=#{repositoryLocalRoot}")

    autorest args,() =>
      instances--
      return done() if instances is 0 

defaultMappings = {
  'AcceptanceTests/ParameterFlattening': 'parameter-flattening.json',
  'AcceptanceTests/BodyArray': 'body-array.json',
  'AcceptanceTests/BodyBoolean': 'body-boolean.json',
  'AcceptanceTests/BodyByte': 'body-byte.json',
  'AcceptanceTests/BodyComplex': 'body-complex.json',
  'AcceptanceTests/BodyDate': 'body-date.json',
  'AcceptanceTests/BodyDateTime': 'body-datetime.json',
  'AcceptanceTests/BodyDateTimeRfc1123': 'body-datetime-rfc1123.json',
  'AcceptanceTests/BodyDuration': 'body-duration.json',
  'AcceptanceTests/BodyDictionary': 'body-dictionary.json',
  'AcceptanceTests/BodyFile': 'body-file.json',
  'AcceptanceTests/BodyFormData': 'body-formdata.json',
  'AcceptanceTests/BodyInteger': 'body-integer.json',
  'AcceptanceTests/BodyNumber': 'body-number.json',
  'AcceptanceTests/BodyString': 'body-string.json',
  'AcceptanceTests/Header': 'header.json',
  'AcceptanceTests/Http': 'httpInfrastructure.json',
  'AcceptanceTests/Report': 'report.json',
  'AcceptanceTests/RequiredOptional': 'required-optional.json',
  'AcceptanceTests/Url': 'url.json',
  'AcceptanceTests/Validation': 'validation.json',
  'AcceptanceTests/CustomBaseUri': 'custom-baseUrl.json',
  'AcceptanceTests/CustomBaseUriMoreOptions': 'custom-baseUrl-more-options.json',
  'AcceptanceTests/ModelFlattening': 'model-flattening.json',
  'AcceptanceTests/UrlMultiCollectionFormat' : 'url-multi-collectionFormat.json',
  'AcceptanceTests/ExtensibleEnums': 'extensible-enums-swagger.json'
  'AcceptanceTests/AdditionalProperties': 'additionalProperties.json'
}

defaultAzureMappings = {
  'AcceptanceTests/Lro': 'lro.json',
  'AcceptanceTests/Paging': 'paging.json',
  'AcceptanceTests/AzureReport': 'azure-report.json',
  'AcceptanceTests/AzureParameterGrouping': 'azure-parameter-grouping.json',
  'AcceptanceTests/AzureResource': 'azure-resource.json',
  'AcceptanceTests/Head': 'head.json',
  'AcceptanceTests/HeadExceptions': 'head-exceptions.json',
  'AcceptanceTests/SubscriptionIdApiVersion': 'subscriptionId-apiVersion.json',
  'AcceptanceTests/AzureSpecials': 'azure-special-properties.json',
  'AcceptanceTests/CustomBaseUri': 'custom-baseUrl.json'
}

compositeMappings = {
  'AcceptanceTests/CompositeBoolIntClient': 'body-boolean.json;body-integer.json'
}

azureCompositeMappings = {
  'AcceptanceTests/AzureCompositeModelClient': 'complex-model.json;body-complex.json'
}

nodeAzureMappings = {
  'AcceptanceTests/StorageManagementClient': 'storage.json'
}

nodeMappings = {
  'AcceptanceTests/ComplexModelClient': 'complex-model.json'
}

swaggerDir = "node_modules/@microsoft.azure/autorest.testserver/swagger"

task 'regenerate-nodecomposite', '', (done) ->
  regenExpected {
    'outputBaseDir': 'test/vanilla',
    'inputBaseDir': swaggerDir,
    'mappings': compositeMappings,
    'modeler': 'CompositeSwagger',
    'outputDir': 'Expected',
    'language': 'nodejs',
    'nsPrefix': 'Fixtures',
    'flatteningThreshold': '1',
    'override-info.title': "Composite Bool Int",
    'override-info.description': "Composite Swagger Client that represents merging body boolean and body integer swagger clients",
    'generatePackageJson': false,
    'generateReadmeMd': false,
    'generateLicenseTxt': false,
    'sourceCodeFolderPath': ''
  },done
  return null

task 'regenerate-nodeazurecomposite', '', (done) ->
  regenExpected {
    'outputBaseDir': 'test/azure',
    'inputBaseDir': swaggerDir,
    'mappings': azureCompositeMappings,
    'modeler': 'CompositeSwagger',
    'outputDir': 'Expected',
    'language': 'nodejs',
    'azureArm': true,
    'nsPrefix': 'Fixtures',
    'flatteningThreshold': '1',
    'override-info.version': "1.0.0",
    'override-info.title': "Azure Composite Model",
    'override-info.description': "Composite Swagger Client that represents merging body complex and complex model swagger clients",
    'generatePackageJson': false,
    'generateReadmeMd': false,
    'generateLicenseTxt': false,
    'sourceCodeFolderPath': ''
  },done
  return null

task 'regenerate-nodeazure', '', ['regenerate-nodeazurecomposite'], (done) ->
  for p of defaultAzureMappings
    nodeAzureMappings[p] = defaultAzureMappings[p]
  regenExpected {
    'outputBaseDir': 'test/azure',
    'inputBaseDir': swaggerDir,
    'mappings': nodeAzureMappings,
    'outputDir': 'Expected',
    'language': 'nodejs',
    'azureArm': true,
    'nsPrefix': 'Fixtures',
    'flatteningThreshold': '1',
    'generatePackageJson': false,
    'generateReadmeMd': false,
    'generateLicenseTxt': false,
    'sourceCodeFolderPath': ''
  },done
  return null

task 'regenerate-node', '', ['regenerate-nodecomposite'], (done) ->
  for p of defaultMappings
    nodeMappings[p] = defaultMappings[p]
  regenExpected {
    'outputBaseDir': 'test/vanilla',
    'inputBaseDir': swaggerDir,
    'mappings': nodeMappings,
    'outputDir': 'Expected',
    'language': 'nodejs',
    'nsPrefix': 'Fixtures',
    'flatteningThreshold': '1',
    'generatePackageJson': false,
    'generateReadmeMd': false,
    'generateLicenseTxt': false,
    'sourceCodeFolderPath': ''
  },done
  return null

regenerateNodeOptionsDependencies = [
  'regenerate-node-generatepackagejson-vanilla-false',
  'regenerate-node-generatepackagejson-azure-false',
  'regenerate-node-generatereadmemd-vanilla-true',
  'regenerate-node-generatereadmemd-azure-true',
  'regenerate-node-generatelicense-vanilla-true'
  'regenerate-node-generatelicense-vanilla-false',
  'regenerate-node-sourcecodefolderpath-vanilla-sources',
  'regenerate-node-sourcecodefolderpath-azure-sources'
]
task 'regenerate-node-options', '', regenerateNodeOptionsDependencies, (done) ->
  done();

task 'regenerate-node-generatepackagejson-vanilla-false', '', [], (done) ->
  regenExpected {
    'outputBaseDir': 'test/options/generatepackagejson-vanilla-false',
    'inputBaseDir': swaggerDir,
    'mappings': {
      'AcceptanceTests/ParameterFlattening': 'parameter-flattening.json',
    },
    'outputDir': 'Expected',
    'language': 'nodejs',
    'nsPrefix': 'Fixtures',
    'flatteningThreshold': '1',
    'generatePackageJson': false,
    'packageName': 'azure-arm-parameterflattening',
    'packageVersion': '1.2.3'
  },done
  return null

task 'regenerate-node-generatepackagejson-azure-false', '', [], (done) ->
  regenExpected {
    'outputBaseDir': 'test/options/generatepackagejson-azure-false',
    'inputBaseDir': swaggerDir,
    'mappings': {
      'AcceptanceTests/ParameterFlattening': 'parameter-flattening.json',
    },
    'outputDir': 'Expected',
    'language': 'nodejs',
    'nsPrefix': 'Fixtures',
    'flatteningThreshold': '1',
    'generatePackageJson': false,
    'azureArm': true,
    'packageName': 'azure-arm-parameterflattening',
    'packageVersion': '1.0.0-preview'
  },done
  return null

task 'regenerate-node-generatereadmemd-vanilla-true', '', [], (done) ->
  regenExpected {
    'outputBaseDir': 'test/options/generatereadmemd-vanilla-true',
    'inputBaseDir': swaggerDir,
    'mappings': {
      'AcceptanceTests/ParameterFlattening': 'parameter-flattening.json',
    },
    'outputDir': 'Expected',
    'language': 'nodejs',
    'nsPrefix': 'Fixtures',
    'flatteningThreshold': '1',
    'generateReadmeMd': true,
    'packageName': 'azure-arm-parameterflattening',
    'packageVersion': '1.2.3'
  },done
  return null

task 'regenerate-node-generatereadmemd-azure-true', '', [], (done) ->
  regenExpected {
    'outputBaseDir': 'test/options/generatereadmemd-azure-true',
    'inputBaseDir': swaggerDir,
    'mappings': {
      'AcceptanceTests/ParameterFlattening': 'parameter-flattening.json',
    },
    'outputDir': 'Expected',
    'language': 'nodejs',
    'nsPrefix': 'Fixtures',
    'flatteningThreshold': '1',
    'generateReadmeMd': true,
    'azureArm': true,
    'packageName': 'azure-arm-parameterflattening',
    'packageVersion': '1.0.0-preview'
  },done
  return null

task 'regenerate-node-generatelicense-vanilla-false', '', [], (done) ->
  regenExpected {
    'outputBaseDir': 'test/options/generatelicense-vanilla-false',
    'inputBaseDir': swaggerDir,
    'mappings': {
      'AcceptanceTests/ParameterFlattening': 'parameter-flattening.json',
    },
    'outputDir': 'Expected',
    'language': 'nodejs',
    'nsPrefix': 'Fixtures',
    'flatteningThreshold': '1',
    'generateLicenseTxt': false,
    'packageName': 'azure-arm-parameterflattening',
    'packageVersion': '1.0.0-preview'
  },done
  return null

task 'regenerate-node-generatelicense-vanilla-true', '', [], (done) ->
  regenExpected {
    'outputBaseDir': 'test/options/generatelicense-vanilla-true',
    'inputBaseDir': swaggerDir,
    'mappings': {
      'AcceptanceTests/ParameterFlattening': 'parameter-flattening.json',
    },
    'outputDir': 'Expected',
    'language': 'nodejs',
    'nsPrefix': 'Fixtures',
    'flatteningThreshold': '1',
    'generateLicenseTxt': true,
    'packageName': 'azure-arm-parameterflattening',
    'packageVersion': '1.0.0-preview'
  },done
  return null

task 'regenerate-node-sourcecodefolderpath-vanilla-sources', '', [], (done) ->
  regenExpected {
    'outputBaseDir': 'test/options/sourcecodefolderpath-vanilla-sources',
    'inputBaseDir': swaggerDir,
    'mappings': {
      'AcceptanceTests/ParameterFlattening': 'parameter-flattening.json',
    },
    'outputDir': 'Expected',
    'language': 'nodejs',
    'nsPrefix': 'Fixtures',
    'flatteningThreshold': '1',
    'packageName': 'azure-arm-parameterflattening',
    'packageVersion': '1.0.0-preview',
    'sourceCodeFolderPath': 'sources'
  },done
  return null

task 'regenerate-node-sourcecodefolderpath-azure-sources', '', [], (done) ->
  regenExpected {
    'outputBaseDir': 'test/options/sourcecodefolderpath-azure-sources',
    'inputBaseDir': swaggerDir,
    'mappings': {
      'AcceptanceTests/ParameterFlattening': 'parameter-flattening.json',
    },
    'outputDir': 'Expected',
    'language': 'nodejs',
    'nsPrefix': 'Fixtures',
    'flatteningThreshold': '1',
    'azureArm': true,
    'packageName': 'azure-arm-parameterflattening',
    'packageVersion': '1.0.0-preview',
    'sourceCodeFolderPath': 'sources'
  },done
  return null

task 'regenerate', "regenerate expected code for tests", ['regenerate-node', 'regenerate-nodeazure', 'regenerate-node-options'], (done) ->
  done();
