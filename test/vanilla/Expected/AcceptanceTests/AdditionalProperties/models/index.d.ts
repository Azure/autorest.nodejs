/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 *
 * Code generated by Microsoft (R) AutoRest Code Generator.
 * Changes may cause incorrect behavior and will be lost if the code is regenerated.
 */

import * as moment from "moment";

export interface ErrorModel {
  status?: number;
  message?: string;
}

export interface PetAPTrue {
  id: number;
  name?: string;
  readonly status?: boolean;
  /**
   * Describes unknown properties. The value of an unknown property can be of "any" type.
  */
  [additionalPropertyName: string]: any;
}

export interface CatAPTrue extends PetAPTrue {
  friendly?: boolean;
}

export interface PetAPObject {
  id: number;
  name?: string;
  readonly status?: boolean;
  /**
   * Describes unknown properties. The value of an unknown property can be of "any" type.
  */
  [additionalPropertyName: string]: any;
}

export interface PetAPString {
  id: number;
  name?: string;
  readonly status?: boolean;
  /**
   * Describes unknown properties. The value of an unknown property MUST be of type "string". Due
   * to valid TS constraints we have modeled this as a union of `string | any`.
  */
  [additionalPropertyName: string]: string | any;
}

export interface PetAPInProperties {
  id: number;
  name?: string;
  readonly status?: boolean;
  additionalProperties?: { [propertyName: string]: number };
}

export interface PetAPInPropertiesWithAPString {
  id: number;
  name?: string;
  readonly status?: boolean;
  odatalocation: string;
  additionalProperties1?: { [propertyName: string]: number };
  /**
   * Describes unknown properties. The value of an unknown property MUST be of type "string". Due
   * to valid TS constraints we have modeled this as a union of `string | any`.
  */
  [additionalPropertyName: string]: string | any;
}
