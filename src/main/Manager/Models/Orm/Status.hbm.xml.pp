<?xml version="1.0" encoding="utf-8"?>

<hibernate-mapping xmlns="urn:nhibernate-mapping-2.2">
  <class name="$rootnamespace$.Models.Status, $rootnamespace$" table="status">
    <id name="Id">
      <generator class="identity" />
    </id>

    <property name="Name" length="50" not-null="true" />
    <property name="Created" not-null="true" />
    <property name="Updated" not-null="true" />
  </class>
</hibernate-mapping>