<?xml version="1.0" encoding="utf-8"?>

<hibernate-mapping xmlns="urn:nhibernate-mapping-2.2">
  <class name="$rootnamespace$.Models.Module, $rootnamespace$" table="modules">
    <id name="Id">
      <generator class="identity" />
    </id>

    <many-to-one name="Parent" column="ParentId" />

    <property name="Name" length="100" not-null="true" />
    <property name="Description" length="100" />
    <property name="Order" column="Module_Order" not-null="true" />
    <property name="Created" not-null="true" />
    <property name="Updated" not-null="true" />

    <bag name="Childs" order-by="Module_Order" lazy="true">
      <key column="ParentId" />
      <one-to-many class="$rootnamespace$.Models.Module, $rootnamespace$" />
    </bag>

    <bag name="Functionalities" cascade="all" order-by="Name" lazy="true">
      <key column="ModuleId" />
      <one-to-many class="$rootnamespace$.Models.Functionality, $rootnamespace$" />
    </bag>    
  </class>
</hibernate-mapping>