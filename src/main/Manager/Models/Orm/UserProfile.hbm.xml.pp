<?xml version="1.0" encoding="utf-8"?>

<hibernate-mapping xmlns="urn:nhibernate-mapping-2.2">
  <class name="$rootnamespace$.Models.UserProfile, $rootnamespace$" table="user_profiles">
    <id name="Id">
      <generator class="identity" />
    </id>
    
    <many-to-one name="State" column="StateId" not-null="false" />

    <property name="Name" length="100" not-null="true" unique="true" />

    <bag name="Functionalities" table="user_profiles_x_functionalities" lazy="true">
      <key column="UserProfileId" />
      <many-to-many class="$rootnamespace$.Models.Functionality, $rootnamespace$" column="FunctionalityId" />
    </bag>
  </class>
  <query name="user-profiles-can-remove">
    <![CDATA[
      select u.Id from User u where u.Profile = :profile
      ]]>
  </query>  
</hibernate-mapping>