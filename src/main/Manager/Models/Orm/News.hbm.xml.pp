<?xml version="1.0" encoding="utf-8"?>

<hibernate-mapping xmlns="urn:nhibernate-mapping-2.2">
  <class name="$rootnamespace$.Models.News, $rootnamespace$" table="news">
    <id name="Id">
      <generator class="identity" />
    </id>

    <many-to-one name="Sponsor" column="SponsorId" not-null="true" />

    <property name="Title" length="100" not-null="true" />
    <property name="Call" column="News_Call" length="500" not-null="true" />
    <property name="Author" length="500" />
    <property name="Date" column="News_Date" not-null="true" />

    <property name="Content" not-null="true">
      <column name="News_Content" sql-type="TEXT" />
    </property>

    <property name="Created" not-null="true" />
    <property name="Updated" not-null="true" />
  </class>
</hibernate-mapping>