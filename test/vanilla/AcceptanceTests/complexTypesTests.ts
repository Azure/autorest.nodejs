﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

'use strict';

var should = require('should');
var assert = require('assert');
import * as msRest from 'ms-rest';
import * as moment from 'moment';
import { AutoRestComplexTestService, AutoRestComplexTestServiceModels } from '../Expected/AcceptanceTests/BodyComplex/autoRestComplexTestService';
import { AdditionalPropertiesClient, AdditionalPropertiesModels } from '../Expected/AcceptanceTests/AdditionalProperties/additionalPropertiesClient';

var clientOptions = {};
var baseUri = 'http://localhost:3000';

describe('nodejs', function () {

  describe('Swagger Complex Type BAT', function () {

    describe('Basic Types Operations', function () {
      var testClient = new AutoRestComplexTestService(baseUri, clientOptions);
      it('should get and put valid basic type properties', function (done) {
        testClient.basicOperations.getValid(function (error, result) {
          should.not.exist(error);
          result.id.should.equal(2);
          result.name.should.equal('abc');
          result.color.should.equal('YELLOW');
          testClient.basicOperations.putValid({ 'id': 2, 'name': 'abc', color: 'Magenta' }, function (error) {
            should.not.exist(error);
            done();
          });
        });
      });

      it('should get null basic type properties', function (done) {
        testClient.basicOperations.getNull(function (error, result) {
          should.not.exist(error);
          assert.equal(null, result.id);
          assert.equal(null, result.name);
          done();
        });
      });

      it('should get empty basic type properties', function (done) {
        testClient.basicOperations.getEmpty(function (error, result) {
          should.not.exist(error);
          should.not.exist(result.id);
          should.not.exist(result.name);
          done();
        });
      });

      it('should get basic type properties when the payload is empty', function (done) {
        testClient.basicOperations.getNotProvided(function (error, result) {
          should.not.exist(error);
          should.not.exist(result);
          done();
        });
      });

      it('should deserialize invalid basic types without throwing', function (done) {
        testClient.basicOperations.getInvalid(function (error, result) {
          should.not.exist(error);
          should.exist(result);
          done();
        });
      });

    });

    describe('Primitive Types Operations', function () {
      var testClient = new AutoRestComplexTestService(baseUri, clientOptions);
      it('should get and put valid int properties', function (done) {
        testClient.primitive.getInt(function (error, result) {
          should.not.exist(error);
          result.field1.should.equal(-1);
          result.field2.should.equal(2);
          testClient.primitive.putInt({ 'field1': -1, 'field2': 2 }, function (error) {
            should.not.exist(error);
            done();
          });
        });
      });

      it('should get and put valid long properties', function (done) {
        testClient.primitive.getLong(function (error, result) {
          should.not.exist(error);
          result.field1.should.equal(1099511627775);
          result.field2.should.equal(-999511627788);
          testClient.primitive.putLong({ 'field1': 1099511627775, 'field2': -999511627788 }, function (error) {
            should.not.exist(error);
            done();
          });
        });
      });

      it('should get and put valid float properties', function (done) {
        testClient.primitive.getFloat(function (error, result) {
          should.not.exist(error);
          result.field1.should.equal(1.05);
          result.field2.should.equal(-0.003);
          testClient.primitive.putFloat({ 'field1': 1.05, 'field2': -0.003 }, function (error) {
            should.not.exist(error);
            done();
          });
        });
      });

      it('should get and put valid double properties', function (done) {
        testClient.primitive.getDouble(function (error, result) {
          should.not.exist(error);
          result.field1.should.equal(3e-100);
          result.field56ZerosAfterTheDotAndNegativeZeroBeforeDotAndThisIsALongFieldNameOnPurpose.should.equal(-0.000000000000000000000000000000000000000000000000000000005);
          testClient.primitive.putDouble({ 'field1': 3e-100, 'field56ZerosAfterTheDotAndNegativeZeroBeforeDotAndThisIsALongFieldNameOnPurpose': -0.000000000000000000000000000000000000000000000000000000005 }, function (error) {
            should.not.exist(error);
            done();
          });
        });
      });

      it('should get and put valid bool properties', function (done) {
        testClient.primitive.getBool(function (error, result) {
          should.not.exist(error);
          result.fieldTrue.should.equal(true);
          result.fieldFalse.should.equal(false);
          testClient.primitive.putBool({ 'fieldTrue': true, 'fieldFalse': false }, function (error) {
            should.not.exist(error);
            done();
          });
        });
      });

      it('should get and put valid string properties', function (done) {
        testClient.primitive.getString(function (error, result) {
          should.not.exist(error);
          result.field.should.equal('goodrequest');
          result.empty.should.equal('');
          should.not.exist(result['nullProperty']);
          testClient.primitive.putString({ 'field': 'goodrequest', 'empty': '' }, function (error) {
            should.not.exist(error);
            done();
          });
        });
      });

      it('should get and put valid date properties', function (done) {
        testClient.primitive.getDate(function (error, result) {
          should.not.exist(error);
          assert.deepEqual(result.field, new Date('0001-01-01'));
          assert.deepEqual(result.leap, new Date('2016-02-29'));
          var complexBody = <AutoRestComplexTestServiceModels.DateWrapper>{ 'field': new Date('0001-01-01'), 'leap': new Date('2016-02-29') }
          testClient.primitive.putDate(complexBody, function (error) {
            should.not.exist(error);
            done();
          });
        });
      });
      it('should get and put valid date-time properties', function (done) {
        testClient.primitive.getDateTime(function (error, result) {
          should.not.exist(error);
          assert.deepEqual(result.field, new Date('0001-01-01T00:00:00Z'));
          assert.deepEqual(result.now, new Date('2015-05-18T18:38:00Z'));
          testClient.primitive.putDateTime({ 'field': new Date('0001-01-01T00:00:00Z'), 'now': new Date('2015-05-18T18:38:00Z') }, function (error) {
            should.not.exist(error);
            done();
          });
        });
      });

      it('should get and put valid date-time-rfc1123 properties', function (done) {
        var timeStringOne = 'Mon, 01 Jan 0001 00:00:00 GMT';
        var timeStringTwo = 'Mon, 18 May 2015 11:38:00 GMT';
        testClient.primitive.getDateTimeRfc1123(function (error, result) {
          should.not.exist(error);
          assert.deepEqual(result.field, new Date(timeStringOne));
          assert.deepEqual(result.now, new Date(timeStringTwo));
          var dateFormat = 'ddd, DD MMM YYYY HH:mm:ss';

          //Have to use moment.js to construct the date object because NodeJS default Date constructor doesn't parse "old" RFC dates right
          var fieldDate = moment.utc(timeStringOne, dateFormat).toDate();
          testClient.primitive.putDateTimeRfc1123({ 'field': fieldDate, 'now': new Date(timeStringTwo) }, function (error) {
            should.not.exist(error);
            done();
          });
        });
      });

      it('should get and put valid duration properties', function (done) {
        var durationString = 'P123DT22H14M12.011S';
        testClient.primitive.getDuration(function (error, result) {
          should.not.exist(error);
          //should.not.exist(result.field);
          assert.deepEqual(result.field.toJSON(), moment.duration(durationString).toJSON());
          testClient.primitive.putDuration({ field: moment.duration(durationString) }, function (error) {
            should.not.exist(error);
            done();
          });
        });
      });

      it('should get and put valid byte properties', function (done) {
        var byteBuffer = new Buffer([255, 254, 253, 252, 0, 250, 249, 248, 247, 246]);
        testClient.primitive.getByte(function (error, result) {
          should.not.exist(error);
          assert.deepEqual(result.field, byteBuffer);
          testClient.primitive.putByte({ field: byteBuffer }, function (error) {
            should.not.exist(error);
            done();
          });
        });
      });

    });

    describe('Array Types Operations', function () {
      var testClient = new AutoRestComplexTestService(baseUri, clientOptions);
      it('should get valid array type properties', function (done) {
        var testArray = ['1, 2, 3, 4', '', null, '&S#$(*Y', 'The quick brown fox jumps over the lazy dog'];
        testClient.arrayModel.getValid(function (error, result) {
          should.not.exist(error);
          assert.deepEqual(result.arrayProperty, testArray);
          testClient.arrayModel.putValid({ arrayProperty: testArray }, function (error) {
            should.not.exist(error);
            done();
          });
        });
      });

      it('should get and put empty array type properties', function (done) {
        testClient.arrayModel.getEmpty(function (error, result) {
          should.not.exist(error);
          assert.deepEqual(result.arrayProperty, []);
          testClient.arrayModel.putEmpty({ arrayProperty: [] }, function (error) {
            should.not.exist(error);
            done();
          });
        });
      });

      it('should get array type properties when the payload is empty', function (done) {
        testClient.arrayModel.getNotProvided(function (error, result) {
          should.not.exist(error);
          should.not.exist(result.arrayProperty);
          done();
        });
      });
    });

    describe('Dictionary Types Operations', function () {
      var testClient = new AutoRestComplexTestService(baseUri, clientOptions);
      it('should get and put valid dictionary type properties', function (done) {
        var testDictionary: { [propertyName: string]: string } =
          { 'txt': 'notepad', 'bmp': 'mspaint', 'xls': 'excel', 'exe': '', '': null };
        testClient.dictionary.getValid(function (error, result) {
          should.not.exist(error);
          assert.deepEqual(result.defaultProgram, testDictionary);
          testClient.dictionary.putValid({ defaultProgram: testDictionary }, function (error) {
            should.not.exist(error);
            done();
          });
        });
      });

      it('should get and put empty dictionary type properties', function (done) {
        testClient.dictionary.getEmpty(function (error, result) {
          should.not.exist(error);
          assert.deepEqual(result.defaultProgram, {});
          testClient.dictionary.putEmpty({ defaultProgram: {} }, function (error) {
            should.not.exist(error);
            done();
          });
        });
      });

      it('should get null dictionary type properties', function (done) {
        testClient.dictionary.getNull(function (error, result) {
          should.not.exist(error);
          should.not.exist(result.defaultProgram);
          done();
        });
      });

      it('should get dictionary type properties when the payload is empty', function (done) {
        testClient.dictionary.getNotProvided(function (error, result) {
          should.not.exist(error);
          should.not.exist(result.defaultProgram);
          done();
        });
      });

    });

    describe('Complex Types with Inheritance Operations', function () {
      var siamese = { "breed": "persian", "color": "green", "hates": [{ "food": "tomato", "id": 1, "name": "Potato" }, { "food": "french fries", "id": -1, "name": "Tomato" }], "id": 2, "name": "Siameeee" };
      var testClient = new AutoRestComplexTestService(baseUri, clientOptions);
      it('should get valid basic type properties', function (done) {
        testClient.inheritance.getValid(function (error, result) {
          should.not.exist(error);
          assert.deepEqual(result, siamese);
          testClient.inheritance.putValid(siamese, function (error) {
            should.not.exist(error);
            done();
          });
        });
      });

    });

    describe('Complex Types with ReadOnly Properties', function () {
      var testClient = new AutoRestComplexTestService(baseUri, clientOptions);
      it('should get and put complex types with readonly properties', function (done) {
        testClient.readonlyproperty.getValid(function (error, result) {
          should.not.exist(error);
          testClient.readonlyproperty.putValid(result, function (error: msRest.ServiceError) {
            should.not.exist(error);
            done();
          });
        });
      });

    });

    describe('Complex Types with Polymorphism Operations', function () {
      var fish = {
        'fishtype': 'salmon',
        'location': 'alaska',
        'iswild': true,
        'species': 'king',
        'length': 1.0,
        'siblings': [
          {
            'fishtype': 'shark',
            'age': 6,
            'birthday': new Date('2012-01-05T01:00:00Z'),
            'length': 20.0,
            'species': 'predator'
          },
          {
            'fishtype': 'sawshark',
            'age': 105,
            'birthday': new Date('1900-01-05T01:00:00Z'),
            'length': 10.0,
            'picture': new Buffer([255, 255, 255, 255, 254]),
            'species': 'dangerous'
          },
          {
            'fishtype': 'goblin',
            'age': 1,
            'length': 30,
            'species': 'scary',
            'birthday': new Date('2015-08-08T00:00:00Z'),
            'jawsize': 5,
            'color': 'pinkish-gray'
          }
        ]
      };
      var testClient = new AutoRestComplexTestService(baseUri, clientOptions);
      it('should get valid polymorphic properties', function (done) {
        testClient.polymorphism.getValid(function (error, result) {
          should.not.exist(error);
          assert.deepEqual(result, fish);
          testClient.polymorphism.putValid(fish, function (error: msRest.ServiceError) {
            should.not.exist(error);
            done();
          });
        });
      });
      var badfish = {
        'fishtype': 'sawshark',
        'species': 'snaggle toothed',
        'length': 18.5,
        'age': 2,
        'birthday': new Date('2013-06-01T01:00:00Z'),
        'location': 'alaska',
        'picture': new Buffer([255, 255, 255, 255, 254]),
        'siblings': [
          {
            'fishtype': 'shark',
            'species': 'predator',
            'birthday': new Date('2012-01-05T01:00:00Z'),
            'length': 20,
            'age': 6
          },
          {
            'fishtype': 'sawshark',
            'species': 'dangerous',
            'picture': new Buffer([255, 255, 255, 255, 254]),
            'length': 10,
            'age': 105
          }
        ]
      };
      it('should throw when required fields are omitted from polymorphic types', function (done) {
        testClient.polymorphism.putValidMissingRequired(badfish, function (error: msRest.ServiceError) {
          should.exist(error);
          error.message.should.containEql('birthday');
          error.message.should.containEql('cannot be null or undefined');
          done();
        });
      });

      var rawSalmon: AutoRestComplexTestServiceModels.SmartSalmon = {
        "species": "king",
        "length": 1,
        "siblings": [
          <AutoRestComplexTestServiceModels.Shark>{
            "species": "predator",
            "length": 20,
            "fishtype": "shark",
            "age": 6,
            "birthday": new Date("2012-01-05T01:00:00.000Z")
          },
          <AutoRestComplexTestServiceModels.Sawshark>{
            "species": "dangerous",
            "length": 10,
            "fishtype": "sawshark",
            "age": 105,
            "birthday": new Date("1900-01-05T01:00:00.000Z"),
            "picture": new Buffer([255, 255, 255, 255, 254])
          },
          <AutoRestComplexTestServiceModels.Goblinshark>{
            "species": "scary",
            "length": 30,
            "fishtype": "goblin",
            "age": 1,
            "birthday": new Date("2015-08-08T00:00:00.000Z"),
            "jawsize": 5,
            "color": "pinkish-gray"
          }
        ],
        "fishtype": "smart_salmon",
        "location": "alaska",
        "iswild": true,
        "additionalProperty1": 1,
        "additionalProperty2": false,
        "additionalProperty3": "hello",
        "additionalProperty4": {
          "a": 1,
          "b": 2
        },
        "additionalProperty5": [
          1,
          3
        ],
      };

      it('should get complicated polymorphic types', function (done) {
        testClient.polymorphism.getComplicated(function (err, result, req, res) {
          should.not.exist(err);
          assert.deepEqual(result, rawSalmon);
          done();
        });
      });

      it('should put complicated polymorphic types', function (done) {
        testClient.polymorphism.putComplicated(rawSalmon, function (err, result, req, res) {
          should.not.exist(err);
          res.statusCode.should.equal(200);
          let serializedPayload = JSON.parse(req['body']);
          serializedPayload.additionalProperty1.should.equal(1);
          serializedPayload.additionalProperty2.should.equal(false);
          serializedPayload.additionalProperty3.should.equal('hello');
          assert.deepEqual(serializedPayload.additionalProperty4, { "a": 1, "b": 2 });
          assert.deepEqual(serializedPayload.additionalProperty5, [1, 3]);
          done();
        });
      });

      it('should put polymorphic types without discriminator property', function (done) {
        var rawSalmon: AutoRestComplexTestServiceModels.SmartSalmon = {
          "species": "king",
          "length": 1,
          "siblings": [
            <AutoRestComplexTestServiceModels.Shark>{
              "species": "predator",
              "length": 20,
              "fishtype": "shark",
              "age": 6,
              "birthday": new Date("2012-01-05T01:00:00.000Z")
            },
            <AutoRestComplexTestServiceModels.Sawshark>{
              "species": "dangerous",
              "length": 10,
              "fishtype": "sawshark",
              "age": 105,
              "birthday": new Date("1900-01-05T01:00:00.000Z"),
              "picture": new Buffer([255, 255, 255, 255, 254])
            },
            <AutoRestComplexTestServiceModels.Goblinshark>{
              "species": "scary",
              "length": 30,
              "fishtype": "goblin",
              "age": 1,
              "birthday": new Date("2015-08-08T00:00:00.000Z"),
              "jawsize": 5,
              "color": "pinkish-gray"
            }
          ],
          "location": "alaska",
          "iswild": true,
          "additionalProperty1": 1,
          "additionalProperty2": false,
          "additionalProperty3": "hello",
          "additionalProperty4": {
            "a": 1,
            "b": 2
          },
          "additionalProperty5": [
            1,
            3
          ],
        } as any;
        testClient.polymorphism.putMissingDiscriminator(rawSalmon, function (err, result, req, res) {
          should.not.exist(err);
          res.statusCode.should.equal(200);
          let serializedPayload = JSON.parse(req['body']);
          serializedPayload.fishtype.should.equal("salmon");
          result.fishtype.should.equal("salmon");
          should.not.exist((result as any).additionalProperty1);
          done();
        });
      });
    });

    describe('Complex Types with recursive definitions', function () {
      var bigfish = <AutoRestComplexTestServiceModels.Fish>{
        'fishtype': 'salmon',
        'location': 'alaska',
        'iswild': true,
        'species': 'king',
        'length': 1,
        'siblings': [
          <AutoRestComplexTestServiceModels.Shark>{
            'fishtype': 'shark',
            'age': 6,
            'birthday': new Date('2012-01-05T01:00:00Z'),
            'species': 'predator',
            'length': 20,
            'siblings': [
              <AutoRestComplexTestServiceModels.Salmon>{
                'fishtype': 'salmon',
                'location': 'atlantic',
                'iswild': true,
                'species': 'coho',
                'length': 2,
                'siblings': [
                  <AutoRestComplexTestServiceModels.Shark>{
                    'fishtype': 'shark',
                    'age': 6,
                    'birthday': new Date('2012-01-05T01:00:00Z'),
                    'species': 'predator',
                    'length': 20
                  },
                  <AutoRestComplexTestServiceModels.Sawshark>{
                    'fishtype': 'sawshark',
                    'age': 105,
                    'birthday': new Date('1900-01-05T01:00:00Z'),
                    'picture': new Buffer([255, 255, 255, 255, 254]),
                    'species': 'dangerous',
                    'length': 10
                  }
                ]
              },
              <AutoRestComplexTestServiceModels.Sawshark>{
                'fishtype': 'sawshark',
                'age': 105,
                'birthday': new Date('1900-01-05T01:00:00Z'),
                'picture': new Buffer([255, 255, 255, 255, 254]),
                'species': 'dangerous',
                'length': 10,
                'siblings': []
              }
            ]
          },
          <AutoRestComplexTestServiceModels.Sawshark>{
            'fishtype': 'sawshark',
            'age': 105,
            'birthday': new Date('1900-01-05T01:00:00Z'),
            'picture': new Buffer([255, 255, 255, 255, 254]),
            'species': 'dangerous',
            'length': 10,
            'siblings': []
          }
        ]
      };
      var testClient = new AutoRestComplexTestService(baseUri, clientOptions);
      it('should get and put valid basic type properties', function (done) {
        testClient.polymorphicrecursive.getValid(function (error, result) {
          should.not.exist(error);
          assert.deepEqual(result, bigfish);
          testClient.polymorphicrecursive.putValid(bigfish, function (error: msRest.ServiceError) {
            should.not.exist(error);
            done();
          });
        });
      });
    });
  });

  describe('Swagger additionalProperties BAT', function () {
    var testClient = new AdditionalPropertiesClient(baseUri, clientOptions);
    it('should put object with additionalProperties true correctly', function (done) {
      var apTrue: AdditionalPropertiesModels.PetAPTrue = {
        id: 1,
        name: 'Puppy',
        birthdate: new Date('2017-12-13T02:29:51Z'),
        complexProperty: {
          color: 'Red'
        }
      };
      var expectedResult = {
        id: 1,
        name: 'Puppy',
        birthdate: '2017-12-13T02:29:51Z',
        status: true,
        complexProperty: {
          color: 'Red'
        }
      };
      testClient.pets.createAPTrue(apTrue, function (error, result, request, response) {
        should.not.exist(error);
        assert.deepEqual(result, expectedResult);
        done();
      });
    });

    it('should put object with additionalProperties type object correctly', function (done) {
      var apObject: AdditionalPropertiesModels.PetAPTrue = {
        id: 2,
        name: 'Hira',
        siblings: [
          {
            id: 1,
            name: 'Puppy',
            birthdate: '2017-12-13T02:29:51Z',
            complexProperty: {
              color: 'Red'
            }
          }
        ],
        picture: new Buffer([255, 255, 255, 255, 254]).toString('base64')
      };
      var expectedResult = {
        id: 2,
        name: 'Hira',
        status: true,
        siblings: [
          {
            id: 1,
            name: 'Puppy',
            birthdate: '2017-12-13T02:29:51Z',
            complexProperty: {
              color: 'Red'
            }
          }
        ],
        picture: new Buffer([255, 255, 255, 255, 254]).toString('base64')
      };
      testClient.pets.createAPObject(apObject, function (error, result, request, response) {
        should.not.exist(error);
        assert.deepEqual(result, expectedResult);
        done();
      });
    });

    it('should put object with additionalProperties type string correctly', function (done) {
      var apString: AdditionalPropertiesModels.PetAPString = {
        id: 3,
        name: 'Tommy',
        color: 'red',
        weight: "10 kg",
        city: "Bombay"
      };
      var expectedResult = {
        id: 3,
        name: 'Tommy',
        color: 'red',
        weight: "10 kg",
        city: "Bombay",
        status: true
      };
      testClient.pets.createAPString(apString, function (error, result, request, response) {
        should.not.exist(error);
        assert.deepEqual(result, expectedResult);
        done();
      });
    });

    it('should put object with additionalProperties in properties correctly', function (done) {
      var apInProperties: AdditionalPropertiesModels.PetAPInProperties = {
        id: 4,
        name: 'Bunny',
        additionalProperties: {
          height: 5.61,
          weight: 599,
          footsize: 11.5
        }
      };
      var expectedResult = {
        id: 4,
        name: 'Bunny',
        status: true,
        additionalProperties: {
          height: 5.61,
          weight: 599,
          footsize: 11.5
        }
      };
      testClient.pets.createAPInProperties(apInProperties, function (error, result, request, response) {
        should.not.exist(error);
        assert.deepEqual(result, expectedResult);
        done();
      });
    });

    it('should put object with additionalProperties in properties and additionalProperties of type string correctly', function (done) {
      var apInPropertiesWithAPString: AdditionalPropertiesModels.PetAPInPropertiesWithAPString = {
        id: 5,
        name: 'Funny',
        odatalocation: 'westus',
        additionalProperties1: {
          height: 5.61,
          weight: 599,
          footsize: 11.5
        },
        color: 'red',
        city: 'Seattle',
        food: 'tikka masala'
      };
      var expectedResult = {
        id: 5,
        name: 'Funny',
        status: true,
        odatalocation: 'westus',
        additionalProperties1: {
          height: 5.61,
          weight: 599,
          footsize: 11.5
        },
        color: 'red',
        city: 'Seattle',
        food: 'tikka masala'
      };
      testClient.pets.createAPInPropertiesWithAPString(apInPropertiesWithAPString, function (error, result, request, response) {
        should.not.exist(error);
        assert.deepEqual(result, expectedResult);
        done();
      });
    });
  });
});
