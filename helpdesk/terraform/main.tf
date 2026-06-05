terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0"
    }
  }
}

provider "azurerm" {
  features {}
}

variable "resource_group_name" {
  default = "helpdesk-rg"
}
variable "location" {
  default = "East US"
}
variable "app_service_plan_name" {
  default = "helpdesk-plan"
}
variable "app_name" {
  default = "helpdesk-api"
}
variable "mongo_connection_string" {
  sensitive = true
}

resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.location
}

resource "azurerm_service_plan" "plan" {
  name                = var.app_service_plan_name
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  os_type             = "Linux"
  sku_name            = "B1"
}

resource "azurerm_linux_web_app" "api" {
  name                = var.app_name
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  service_plan_id     = azurerm_service_plan.plan.id

  site_config {
    application_stack {
      dotnet_version = "8.0"
    }
  }

  app_settings = {
    "MongoDB__ConnectionString" = var.mongo_connection_string
    "MongoDB__DatabaseName"     = "HelpdeskDB"
    "ASPNETCORE_ENVIRONMENT"    = "Production"
  }
}

output "api_url" {
  value = azurerm_linux_web_app.api.default_hostname
}